using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Sample.Core.Extensions;
using Sample.Core.Logging;
using Sample.Core.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;

namespace Sample.Core.Modules
{
    internal class Bootstrapper
    {
        private System.Web.Mvc.IDependencyResolver _originalResolver;
        private ILifetimeScopeProvider _requestLifetimeScopeProvider;
        private ContainerBuilder _builder;
        private IContainer _applicationContainer;
        private ILogger _logger;

        public SampleWebApplicationConfiguration WebApplicationConfiguration { get; set; }

        public Bootstrapper()
        {
            _logger = new SystemLogger();
            _builder = new ContainerBuilder();
            WebApplicationConfiguration = SampleWebApplicationConfiguration.DefaultConfiguration();
        }

        public IContainer GetApplicationContainer()
        {
            return _applicationContainer;
        }

        public void InitializeForWeb(HttpConfiguration config, SampleWebApplicationConfiguration lamaWebApplicationConfiguration = null)
        {
            var moduleTypesForWebApp = new[] { typeof(ILamaModule), typeof(ILamaWebModule), typeof(ILamaWebApiModule) };

            WebApplicationConfiguration = lamaWebApplicationConfiguration;

            PreloadAssemblies();
            RegisterAllModules(moduleTypesForWebApp);
            BuildContainer();
            RegisterDependancyResolvers(config);
            RegisterWebApiErrorHanding(config);
            ModulesPostInitialization(moduleTypesForWebApp);
        }

        public void InitializeForWorker()
        {
            var moduleTypesForWorker = new[] { typeof(ILamaModule), typeof(ILamaCoreModule) };

            PreloadAssemblies();
            RegisterAllModules(moduleTypesForWorker);
            BuildContainer();
            RegisterWorkerDependancyResolvers();

            ModulesPostInitialization(moduleTypesForWorker);
        }

        private void RegisterWeb(Assembly assembly)
        {
            _builder.RegisterControllers(assembly).PropertiesAutowired();
        }

        private void RegisterWebApi(Assembly assembly)
        {
            _builder.RegisterApiControllers(assembly).PropertiesAutowired();
        }

        internal static IEnumerable<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        private void RegisterAllModules(Type[] interfaces)
        {
            var webApiModuleType = typeof(ILamaWebApiModule);
            var webModuleType = typeof(ILamaWebModule);
            var assembliesInDomain = GetAllAssemblies();

            try
            {
                foreach (var assembly in assembliesInDomain)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!type.IsInterface)
                        {
                            if (webApiModuleType.IsAssignableFrom(type))
                            {
                                RegisterWebApi(assembly);
                            }
                            else if (webModuleType.IsAssignableFrom(type))
                            {
                                RegisterWeb(assembly);
                            }
                        }
                    }
                }

                var modulesInAssemblies = assembliesInDomain
                    .Select(assembly => assembly.GetTypes())
                    .SelectMany(type => type)
                    .Where(type => type.GetInterfaces().Any(interfaceForType => interfaces.Contains(interfaceForType)) && !type.IsInterface)
                    .DistinctBy(type => type.FullName);

                foreach (var module in modulesInAssemblies)
                {
                    var moduleInstance = Activator.CreateInstance(module);
                    var initializeMethod = module.GetMethod("Initialize");

                    if (initializeMethod != null)
                    {
                        try
                        {
                            initializeMethod.Invoke(moduleInstance, new object[] { _builder });
                        }
                        catch (NotImplementedException e)
                        {
                            _logger.Error(e);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                if (e.LoaderExceptions != null) {
                    _logger.Error(e, "{0}", string.Join(" , ", e.LoaderExceptions.ToList()));
                }
                else
                {
                    _logger.Error(e);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }   
        }

        private void ModulesPostInitialization(Type[] interfaces)
        {
            var modulesInAssemblies = GetAllAssemblies()
                .Select(assembly => assembly.GetTypes())
                .SelectMany(type => type)
                .Where(type => type.GetInterfaces().Any(interfaceForType => interfaces.Contains(interfaceForType)) && !type.IsInterface)
                .DistinctBy(type => type.FullName);


            foreach (var module in modulesInAssemblies)
            {
                var moduleInstance = Activator.CreateInstance(module);
                var initializeMethod = module.GetMethod("PostInitialize");

                if (initializeMethod != null)
                {
                    try
                    {
                        initializeMethod.Invoke(moduleInstance, new object[] { _applicationContainer });
                    }
                    catch (NotImplementedException e)
                    {
                        _logger.Error(e);
                    }
                }
            }

        }

        private void BuildContainer()
        {
            _applicationContainer = _builder.Build();
        }

        private void RegisterWebApiErrorHanding(HttpConfiguration config)
        {
            if (config != null)
            {
                var exceptionLogger = DependencyResolver.Current.GetService<IExceptionLogger>();

                if (exceptionLogger != null)
                {
                    config.Services.Add(typeof(IExceptionLogger), exceptionLogger);
                }
            }
        }

        private void RegisterDependancyResolvers(HttpConfiguration config)
        {
            if (WebApplicationConfiguration.RegisterWebApiDependancyInjection && config != null)
            {              
                var webApiResolver = new AutofacWebApiDependencyResolver(_applicationContainer);

                config.DependencyResolver = webApiResolver;
            }

            if (WebApplicationConfiguration.RegisterMvcDependancyInjection)
            {
                var mvcResolver = new Autofac.Integration.Mvc.AutofacDependencyResolver(_applicationContainer);

                DependencyResolver.SetResolver(mvcResolver);
            }
        }

        private void RegisterWorkerDependancyResolvers()
        {
            _requestLifetimeScopeProvider = new RequestLifetimeScopeProvider(_applicationContainer);

            var mvcResolver = new Autofac.Integration.Mvc.AutofacDependencyResolver(_applicationContainer, _requestLifetimeScopeProvider);

            _originalResolver = DependencyResolver.Current;
            DependencyResolver.SetResolver(mvcResolver);
        }

        public void EndLifetimeScope()
        {
            if (_requestLifetimeScopeProvider != null)
            {
                _requestLifetimeScopeProvider.EndLifetimeScope();

                if (_originalResolver != null)
                {
                    DependencyResolver.SetResolver(_originalResolver);
                }
            }
        }

        private void PreloadAssemblies()
        {
            var alreadyLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;

            LoadAssembliesFromPath(baseDirectory, alreadyLoadedAssemblies);

            if (Directory.Exists(privateBinPath))
            {
                LoadAssembliesFromPath(privateBinPath, alreadyLoadedAssemblies);
            }
        }

        private void LoadAssembliesFromPath(string path, Assembly[] alreadyLoadedAssemblies)
        {
            var assemblyFiles = Directory
                .GetFiles(path)
                .Where(file => Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase));

            foreach (var assemblyFile in assemblyFiles)
            {
                var assemblyFileName = Path.GetFileNameWithoutExtension(assemblyFile);

                if (!alreadyLoadedAssemblies.Any(assembly => assembly.GetName().Name.Contains(assemblyFileName)))
                {
                    Assembly.LoadFrom(assemblyFile);
                }
            }
        }
    }
}
