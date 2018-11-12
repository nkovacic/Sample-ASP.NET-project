using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Modules
{
    public interface ILamaModule
    {
        void Initialize(ContainerBuilder builder);
        void PostInitialize(ILifetimeScope container);
    }
}
