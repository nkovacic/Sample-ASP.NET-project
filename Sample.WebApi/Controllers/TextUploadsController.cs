using Sample.Application.Services;
using Sample.Application.ViewModels;
using Sample.Core.Api.Controllers;
using Sample.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.WebApi.Controllers
{
    [RoutePrefix("api/v1/text-uploads")]
    public class TextUploadsController : BaseServiceController
    {
        public TextUploadService ServiceForTextUploads { get; set; }

        [Route("")]
        [HttpPost, ValidateModel]
        public async Task<IHttpActionResult> ReverseLines(TextUploadViewModel uploadViewModel)
        {
            var serviceResult = await ServiceForTextUploads.ReverseLinesFromUpload(uploadViewModel);

            return CreateServiceResult(serviceResult);
        }
    }
}
