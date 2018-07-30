using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public class SmartLogPerTypeByLevel<T> : ConcurrentDictionary<SmartLogLevel, SmartLogLines>
    {
        public SmartLogPerTypeByLevel()
        {
            foreach (SmartLogLevel smartLogLevel in SmartLoggerFactory.SmartLogLevelsWriteOrder)
            {
                string line = SmartLogLine.GetLine("New",
                    smartLogLevel.ToLine(),
                    typeof(T).ToString());
                Log(smartLogLevel,
                    line);
            }
        }


        public void Log(SmartLogLevel smartLogLevel,
            string line)
        {
            SmartLogLines smartLogLines = GetOrAdd(smartLogLevel,
                new SmartLogLines());
            smartLogLines.Log<T>(smartLogLevel,
                line);
        }

        public long Size()
        {
            long size = 0L;
            foreach (SmartLogLevel smartLogLevel in Keys)
            {
                TryGetValue(smartLogLevel,
                    out SmartLogLines smartLogLines);
                size += smartLogLines?.Count
                        ?? 0;
            }

            return size;
        }

        public void Write(TextWriter textWriter)
        {
            foreach (SmartLogLevel smartLogLevel in SmartLoggerFactory.SmartLogLevelsWriteOrder)
            {
                TryGetValue(smartLogLevel,
                    out SmartLogLines smartLogLines);
                smartLogLines?.Write(textWriter);
            }
        }
    }
}