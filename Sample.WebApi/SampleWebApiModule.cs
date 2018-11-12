using Autofac;
using Newtonsoft.Json;
using Sample.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.WebApi
{
    public class SampleWebApiModule : ILamaWebApiModule
    {
        public void Initialize(ContainerBuilder builder)
        {
            
        }

        public void PostInitialize(ILifetimeScope container) { }
    }
}