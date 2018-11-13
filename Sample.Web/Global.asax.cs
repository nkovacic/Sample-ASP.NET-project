using Sample.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sample.Web
{
    public class MvcApplication : SampleWebApplication
    {
        protected void Application_Start()
        {
            base.Application_Start(SampleWebApplicationConfiguration.WebConfiguration());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
