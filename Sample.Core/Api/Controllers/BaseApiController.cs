using Sample.Core.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.Core.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        public IUnitOfWorkAsync UnitOfWork { get; set; }
    }
}
