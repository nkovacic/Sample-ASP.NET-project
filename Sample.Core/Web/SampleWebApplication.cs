using Sample.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sample.Core.Web
{
    public class SampleWebApplicationConfiguration
    {
        public bool RegisterMvcDependancyInjection { get; set; }
        public bool RegisterWebApiDependancyInjection { get; set; }

        public static SampleWebApplicationConfiguration DefaultConfiguration()
        {
            return new SampleWebApplicationConfiguration
            {
                RegisterMvcDependancyInjection = true,
                RegisterWebApiDependancyInjection = true
            };
        }

        public static SampleWebApplicationConfiguration WebConfiguration()
        {
            return new SampleWebApplicationConfiguration
            {
                RegisterMvcDependancyInjection = true,
                RegisterWebApiDependancyInjection = false
            };
        }
    }

    public abstract class SampleWebApplication : HttpApplication
    {
        private Bootstrapper _bootstraper;
        private bool _isInitialized;

        public SampleWebApplication()
        {
            _bootstraper = new Bootstrapper();
        }

        /// <summary>
        /// This method is called by ASP.NET system on web application's startup.
        /// </summary>
        protected virtual void Application_Start(HttpConfiguration config, SampleWebApplicationConfiguration lamaWebApplicationConfiguration)
        {
            if (!_isInitialized)
            {
                _bootstraper.InitializeForWeb(config, lamaWebApplicationConfiguration);
                _isInitialized = true;
            }
        }

        protected virtual void Application_Start(SampleWebApplicationConfiguration lamaWebApplicationConfiguration)
        {
            if (!_isInitialized)
            {
                _bootstraper.InitializeForWeb(null, lamaWebApplicationConfiguration);
                _isInitialized = true;
            }
        }

        /// <summary>
        /// This method is called by ASP.NET system when a request starts.
        /// </summary>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method is called by ASP.NET system when a request ends.
        /// </summary>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {

        }
    }
}
