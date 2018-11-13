using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sample.Core.Logging
{
    class SystemLogger : ILogger
    {
        private void Log(string messageTemplate, LogSeverity logSeverity = LogSeverity.Information,
            Exception exception = null, params object[] propertyValues)
        {
            string log = null;

            if (string.IsNullOrWhiteSpace(messageTemplate))
            {
                if (exception != null)
                {
                    log = exception.StackTrace;
                }
                else
                {
                    return;
                }
            }
            else
            {
                log = string.Format(messageTemplate, propertyValues);
            }

            System.Diagnostics.Debug.WriteLine($"{logSeverity}:{log}");
        }

        public void Debug(Exception e)
        {
            Log(null, LogSeverity.Debug, e);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            Log(messageTemplate, LogSeverity.Debug, propertyValues: propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(messageTemplate, LogSeverity.Debug, exception, propertyValues);
        }

        public void Error(Exception exception)
        {
            Log(null, LogSeverity.Error, exception);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            Log(messageTemplate, LogSeverity.Error, propertyValues: propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(messageTemplate, LogSeverity.Error, exception, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void TrackEvent(string eventName, Dictionary<string, string> eventProperties)
        {
            throw new NotImplementedException();
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }
    }
}
