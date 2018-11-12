using Sample.Core.Utilities;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Logging
{
    public class ApplicationInsightsLogger : ILogger
    {
        private TelemetryClient _client;

        public ApplicationInsightsLogger(TelemetryClient client)
        {
            _client = client;
        }

        public void Debug(Exception exception)
        {
            TrackException(exception);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            TrackTrace(SeverityLevel.Information, messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Debug(exception);
            Debug(messageTemplate, propertyValues);
        }

        public void Error(Exception exception)
        {
            TrackException(exception);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            TrackTrace(SeverityLevel.Error, messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Error(exception);
            Debug(messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            TrackTrace(SeverityLevel.Critical, messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            TrackException(exception);
            Fatal(messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            TrackTrace(SeverityLevel.Information, messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            TrackException(exception);
            Information(messageTemplate, propertyValues);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> eventProperties)
        {
            if (_client != null)
            {
                _client.TrackEvent(eventName, eventProperties);
            }
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            TrackTrace(SeverityLevel.Verbose, messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            TrackException(exception);
            Verbose(messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            TrackTrace(SeverityLevel.Warning, messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            TrackException(exception);          
            Warning(messageTemplate, propertyValues);
        }

        private void TrackException(Exception exception)
        {
            if (exception != null && _client != null)
            {
                var lineNumber = DebugHelper.GetExceptionLineNumber(exception);

                _client.TrackException(exception, new Dictionary<string, string> { { "lineNumber", lineNumber.ToString() } });
                _client.TrackTrace($"Exception {exception.Message} has been thrown on line number {lineNumber}!");
            }
        }

        private void TrackTrace(SeverityLevel severityLevel, string messageTemplate, params object[] propertyValues)
        {
            var message = string.Format(messageTemplate, propertyValues);

            if (_client != null)
            {
                _client.TrackTrace(message, severityLevel);
            }
            else
            {
                Trace.TraceWarning(message);
            }
        }
    }
}
