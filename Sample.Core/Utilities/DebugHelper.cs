using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Utilities
{
    public class DebugHelper
    {
        public static int GetExceptionLineNumber(Exception e)
        {
            if (e != null)
            {
                var stackTrace = new StackTrace(e, true);
                var maxFrameCount = Math.Max(stackTrace.FrameCount, 30);

                for (int i = 0; i < stackTrace.FrameCount; i++)
                {
                    var frame = stackTrace.GetFrame(i);

                    if (frame.GetFileLineNumber() > 0)
                    {
                        return frame.GetFileColumnNumber();
                    }
                }
            }

            return -1;
        }
    }
}
