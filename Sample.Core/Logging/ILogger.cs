using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Logging
{
    public interface ILogger
    {
        void Debug(Exception e);
        void Debug(string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Debug level and associated
        //     exception.
        //
        // Parameters:
        //   exception:
        //     Exception related to the event.
        //
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Error level and associated
        //     exception.
        //
        // Parameters:
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        void Error(Exception exception);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Error level and associated
        //     exception.
        //
        // Parameters:
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        void Error(string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Error level and associated
        //     exception.
        //
        // Parameters:
        //   exception:
        //     Exception related to the event.
        //
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Fatal level and associated
        //     exception.
        //
        // Parameters:
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Fatal(string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Fatal level and associated
        //     exception.
        //
        // Parameters:
        //   exception:
        //     Exception related to the event.
        //
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Information level
        //     and associated exception.
        //
        // Parameters:
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Information(string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Information level
        //     and associated exception.
        //
        // Parameters:
        //   exception:
        //     Exception related to the event.
        //
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Information(Exception exception, string messageTemplate, params object[] propertyValues);

        void TrackEvent(string eventName, Dictionary<string, string> eventProperties);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Verbose level and
        //     associated exception.
        //
        // Parameters:
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.

        void Verbose(string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Verbose level and
        //     associated exception.
        //
        // Parameters:
        //   exception:
        //     Exception related to the event.
        //
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Verbose(Exception exception, string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Warning level and
        //     associated exception.
        //
        // Parameters:
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Warning(string messageTemplate, params object[] propertyValues);
        //
        // Summary:
        //     Write a log event with the Serilog.Events.LogEventLevel.Warning level and
        //     associated exception.
        //
        // Parameters:
        //   exception:
        //     Exception related to the event.
        //
        //   messageTemplate:
        //     Message template describing the event.
        //
        //   propertyValues:
        //     Objects positionally formatted into the message template.
        
        void Warning(Exception exception, string messageTemplate, params object[] propertyValues);
    }
}
