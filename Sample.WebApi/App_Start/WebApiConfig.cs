using Sample.Core;
using Sample.Core.Web.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;

namespace Sample.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            var multipartFormFormatter = DependencyResolver.Current.GetService<MultipartFormFormatter>();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            jsonFormatter.SerializerSettings = SampleCoreModule.JsonSettings;
            config.Formatters.Add(multipartFormFormatter);
        }
    }
}
