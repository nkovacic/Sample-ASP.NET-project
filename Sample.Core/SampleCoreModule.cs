using Autofac;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Core.Data.Database;
using Sample.Core.Logging;
using Sample.Core.Modules;
using Sample.Core.Web.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Sample.Core
{
    public class SampleCoreModule : ILamaCoreModule, ILamaWebApiModule, ILamaWebModule
    {
        public static JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter> {
                new StringEnumConverter { CamelCaseText = true },
                new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz"  }
            },
            DateParseHandling = DateParseHandling.DateTimeOffset,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        };

        public void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationInsightsLogger>().As<ILogger>().SingleInstance();
            builder.RegisterType<ApplicationInsightsWebApiLogger>().As<IExceptionLogger>().SingleInstance();
            builder.RegisterType<MultipartFormFormatter>().SingleInstance();
            builder.RegisterType<TelemetryClient>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }

        public void PostInitialize(ILifetimeScope container) { }
    }
}
