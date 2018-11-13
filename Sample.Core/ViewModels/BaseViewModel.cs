using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.ViewModels
{
    public class BaseViewModel
    {
        public string FormatDateWithTime(DateTimeOffset dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm");
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, SampleCoreModule.JsonSettings);
        }

        public string ToJson(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, SampleCoreModule.JsonSettings);
        }

        public string ToJson(string propertyName)
        {
            var propertyInfo = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(property => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(this);

                if (value != null)
                {
                    return JsonConvert.SerializeObject(value, SampleCoreModule.JsonSettings);
                }
            }

            return "";
        }
    }
}
