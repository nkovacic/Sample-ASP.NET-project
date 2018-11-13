using Sample.Application.Models;
using Sample.Application.Services;
using Sample.Core.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.WebApi.Controllers
{
    public class HomeController : BaseServiceController
    {
        //warmup
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Home()
        {
            await UnitOfWork
                .RepositoryAsync<Dictionary>()
                .Query()
                .SelectPageAsync(1, 10);

            return Ok();
        }
    }
}
