using Sample.Core.Utilities;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;

namespace Sample.Core.Logging
{
    public class ApplicationInsightsWebApiLogger : ExceptionLogger
    {
        private TelemetryClient _aiTelemetryClient;

        public ApplicationInsightsWebApiLogger(TelemetryClient aiTelemetryClient)
        {
            _aiTelemetryClient = aiTelemetryClient;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                var lineNumber = DebugHelper.GetExceptionLineNumber(context.Exception);

                _aiTelemetryClient.TrackException(context.Exception, new Dictionary<string, string> { { "lineNumber", lineNumber.ToString() } });
                _aiTelemetryClient.TrackTrace($"Exception {context.Exception.Message} has been thrown on line number {lineNumber}!");
            }

            base.Log(context);
        }
    }
}
