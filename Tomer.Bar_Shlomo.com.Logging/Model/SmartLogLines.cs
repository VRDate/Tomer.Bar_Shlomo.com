using System.Collections.Concurrent;
using System.IO;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    public class SmartLogLines : ConcurrentQueue<SmartLogLine>
    {
        public void Log<T>(SmartLogLevel smartLogLevel,
            string line)
        {
            var smartLogLine = smartLogLevel.CreateSmartLogLine<T>(line);
            Enqueue(smartLogLine);
        }

        public void Write(TextWriter textWriter)
        {
            while (TryDequeue(out var smartLogLine)) smartLogLine?.Write(textWriter);
        }
    }
}