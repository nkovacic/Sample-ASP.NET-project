using Sample.Core.Logging;
using Sample.Core.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.Core.Api.Controllers
{
    public class BaseServiceController : BaseApiController
    {
        public ILogger Logger { get; set; }

        protected virtual IHttpActionResult CreateServiceResult<T>(ServiceResult<T> serviceResult)
        {
            if (serviceResult == null)
            {
                serviceResult = new ServiceResult<T>();
            }

            var responseMessage = new HttpResponseMessage(serviceResult.Status);

            if (serviceResult.PotentialException != null)
            {
                Logger.Error(serviceResult.PotentialException, serviceResult.StatusMessage);

                var exceptionJson = JsonConvert.SerializeObject(serviceResult.PotentialException, SampleCoreModule.JsonSettings);

                responseMessage.Content = new StringContent(exceptionJson, Encoding.UTF8, "application/json");
            }
            else if (!string.IsNullOrWhiteSpace(serviceResult.StatusMessage))
            {
                Logger.Warning(serviceResult.StatusMessage);

                var dynamicServiceResult = new Dictionary<string, object>
                {
                    { "message", serviceResult.StatusMessage }
                };

                if (!string.IsNullOrWhiteSpace(serviceResult.StatusKey))
                {
                    dynamicServiceResult.Add("statusKey", serviceResult.StatusKey);

                    if (serviceResult.StatusKeyValueParameters != null)
                    {
                        dynamicServiceResult.Add("statusKeyValueParameters", serviceResult.StatusKeyValueParameters);
                    }
                    else if (serviceResult.StatusParameters != null)
                    {
                        dynamicServiceResult.Add("statusParameters", serviceResult.StatusParameters);
                    }
                }

                var statusMessageJson = JsonConvert.SerializeObject(dynamicServiceResult, SampleCoreModule.JsonSettings);

                responseMessage.Content = new StringContent(statusMessageJson, Encoding.UTF8, "application/json");
            }
            else if (serviceResult.Data != null && typeof(T) != typeof(bool))
            {
                var dataMessageJson = JsonConvert.SerializeObject(serviceResult.Data, SampleCoreModule.JsonSettings);

                responseMessage.Content = new StringContent(dataMessageJson, Encoding.UTF8, "application/json");
            }

            return ResponseMessage(responseMessage);
        }
    }
}