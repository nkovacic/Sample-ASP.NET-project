using Sample.Core.Data.Database;
using Sample.Core.Logging;
using System.Security.Claims;
using System.Web.Mvc;

namespace Sample.Core.Web.Controllers
{
    public class BaseWebController: Controller
    {
        public ILogger Logger { get; set; }
        public IUnitOfWorkAsync UnitOfWork { get; set; }

        public ActionResult EmptyView()
        {
            return View("EmptyView");
        }

        public ActionResult EmptyView(object model)
        {
            return View("EmptyView", model);
        }
    }
}
