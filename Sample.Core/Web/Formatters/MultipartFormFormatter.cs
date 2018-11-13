using Sample.Core.Expressions;
using Sample.Core.Files;
using Sample.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Sample.Core.Web.Formatters
{
    public class MultipartFormFormatter : FormUrlEncodedMediaTypeFormatter
    {
        private const string StringMultipartMediaType = "multipart/form-data";
        private const string StringApplicationMediaType = "application/octet-stream";
        private ILogger _logger;

        public MultipartFormFormatter(ILogger logger)
        {
            _logger = logger;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(StringMultipartMediaType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(StringApplicationMediaType));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            MultipartMemoryStreamProvider parts = null;

            try
            {
                parts = await content.ReadAsMultipartAsync();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Client was disconnected while uploading file!");
            }

            var obj = Activator.CreateInstance(type);

            if (parts != null)
            {                
                var propertiesFromObj = obj.GetType().GetRuntimeProperties().ToList();

                foreach (var property in propertiesFromObj)
                {
                    if (property.PropertyType == typeof(FileModel))
                    {
                        var file = parts.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name.Replace("\"", "").Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                        if (file == null || file.Headers.ContentLength <= 0)
                        {
                            continue;
                        }

                        try
                        {
                            var fileModel = new FileModel(file.Headers.ContentDisposition.FileName.Trim('\"'), file.Headers.ContentType.MediaType, Convert.ToInt32(file.Headers.ContentLength), await file.ReadAsStreamAsync());

                            property.SetValue(obj, fileModel);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, "Error while parsing property {0} for one FileModel.", property.Name);
                        }
                    }
                    else if (ExpressionsHelper.IsPropertyNonStringEnumerable(property.PropertyType) && ExpressionsHelper.GetElementType(property.PropertyType) == typeof(FileModel))
                    {
                        var files = parts.Contents.Where(x => x.Headers.ContentDisposition.Name.Replace("\"", "").StartsWith(property.Name, StringComparison.InvariantCultureIgnoreCase));

                        if (files != null && files.Count() > 0)
                        {
                            var fileModels = new List<FileModel>();

                            foreach (var file in files)
                            {
                                if (file.Headers.ContentDisposition != null && !string.IsNullOrWhiteSpace(file.Headers.ContentDisposition.FileName)
                                    && !string.IsNullOrWhiteSpace(file.Headers.ContentType.MediaType))
                                {
                                    try
                                    {
                                        var fileModel = new FileModel(file.Headers.ContentDisposition.FileName.Trim('\"'), file.Headers.ContentType.MediaType, Convert.ToInt32(file.Headers.ContentLength), await file.ReadAsStreamAsync());

                                        fileModels.Add(fileModel);
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.Error(e, "Error while parsing property {0} for multiple FileModels.", property.Name);
                                    }
                                }
                            }

                            if (property.PropertyType.IsArray)
                            {
                                property.SetValue(obj, fileModels.ToArray());
                            }
                            else
                            {
                                property.SetValue(obj, fileModels);
                            }
                        }
                    }
                }

                foreach (var property in propertiesFromObj.Where(x => ExpressionsHelper.GetElementType(x.PropertyType) != typeof(FileModel)))
                {
                    var formData = parts.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name.Replace("\"", "").Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (formData == null)
                    {
                        continue;
                    }

                    try
                    {
                        var strValue = await formData.ReadAsStringAsync();
                        object value = null;

                        if (!string.IsNullOrWhiteSpace(strValue) && strValue != "null" && strValue != "undefined")
                        {
                            strValue = strValue.Trim('\"');

                            if (property.PropertyType.IsEnum)
                            {
                                value = Enum.Parse(property.PropertyType, strValue, true);
                            }
                            else if (ExpressionsHelper.IsPropertyNonStringEnumerable(property.PropertyType))
                            {
                                value = JsonConvert.DeserializeObject(strValue, property.PropertyType);
                            }
                            else
                            {
                                var valueType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                                value = ExpressionsHelper.GetValueFromJsonString(valueType, strValue);
                            }
                        }

                        property.SetValue(obj, value);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, "Error while parsing property {0} for ViewModel.", property.Name);
                    }
                }
            }

            return obj;
        }
    }
}
