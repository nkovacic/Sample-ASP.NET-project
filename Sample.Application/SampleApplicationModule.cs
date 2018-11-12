using Autofac;
using Sample.Core.Data.Database;
using Sample.Core.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application
{
    public class SampleApplicationModule : ILamaModule
    {
        public void Initialize(ContainerBuilder builder)
        {
            builder
                .RegisterType<DataContext>()
                .As<IDataContext>()
                .As<IDataContextAsync>()
                .WithParameter("nameOrConnectionString", "DbContext")
                .InstancePerLifetimeScope();
        }

        public void PostInitialize(ILifetimeScope container)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Sample.Application.Migrations.Configuration>(true));
        }
    }
}
