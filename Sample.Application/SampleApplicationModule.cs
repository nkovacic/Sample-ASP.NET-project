using Autofac;
using Sample.Application.Services;
using Sample.Core.Data.Database;
using Sample.Core.Expressions;
using Sample.Core.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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

            RegisterServices(builder);
        }

        public void PostInitialize(ILifetimeScope container)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Sample.Application.Migrations.Configuration>(true));
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<BaseService>().PropertiesAutowired().InstancePerLifetimeScope();

            var baseServiceType = typeof(BaseService);
            var applicationServices = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => ExpressionsHelper.HasBaseType(type, baseServiceType));

            foreach (var applicationService in applicationServices)
            {
                builder
                    .RegisterType(applicationService)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .InstancePerLifetimeScope();
            }
        }
    }
}
