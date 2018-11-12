using Autofac;
using Sample.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Web
{
    public class SampleWebModule : ILamaWebModule
    {
        public void Initialize(ContainerBuilder builder)
        {
            
        }

        public void PostInitialize(ILifetimeScope container) { }
    }
}