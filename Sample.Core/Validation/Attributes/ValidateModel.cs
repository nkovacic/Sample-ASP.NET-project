using Sample.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Sample.Core.Validation
{
    public class ValidateModel : System.Web.Http.Filters.ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public ValidateModel() : this(arguments => arguments.ContainsValue(null)) { }

        public ValidateModel(bool emptyAllowed)
        {
            if (!emptyAllowed)
            {
                _validate = arguments => arguments.ContainsValue(null);
            }
        }

        public ValidateModel(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_validate != null && _validate(actionContext.ActionArguments))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The argument cannot be null");

                var logger = DependencyResolver.Current.GetService<ILogger>();

                logger.Warning("Empty argument when it wasn't allowed. UserAgent: {0}", actionContext.Request.Headers.UserAgent);
            }
            else
            {
                var modelState = actionContext.ModelState;

                if (!modelState.IsValid)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);

                    var logger = DependencyResolver.Current.GetService<ILogger>();

                    logger.Warning("Model errors: {0}. UserAgent: {1}", ConvertModelstateToJson(actionContext), actionContext.Request.Headers.UserAgent);
                }
            }
        }

        private string ConvertModelstateToJson(HttpActionContext actionContext)
        {
            if (actionContext.ModelState != null && actionContext.ModelState.Values != null)
            {
                var errorsList = actionContext.ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                return JsonConvert.SerializeObject(errorsList, SampleCoreModule.JsonSettings);
            }

            return string.Empty;
        }
    }
}