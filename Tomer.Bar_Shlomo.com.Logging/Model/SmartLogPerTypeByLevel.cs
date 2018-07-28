using System.Collections.Concurrent;
using System.IO;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    public class SmartLogPerTypeByLevel<T> : ConcurrentDictionary<SmartLogLevel, SmartLogLines>
    {
        private static readonly SmartLogLevel[] SmartLogLevelsWriteOrder =
        {
            SmartLogLevel.Critical,
            SmartLogLevel.Error,
            SmartLogLevel.Warn,
            SmartLogLevel.Info,
            SmartLogLevel.Debug,
            SmartLogLevel.Trace
        };

        public SmartLogPerTypeByLevel()
        {
            foreach (var smartLogLevel in SmartLogLevelsWriteOrder)
            {
                var line = SmartLogLine.GetLine("New",
                    smartLogLevel.ToLine(),
                    typeof(T).ToString());
                Log(smartLogLevel,
                    line);
            }
        }


        public void Log(SmartLogLevel smartLogLevel,
            string line)
        {
            var smartLogLines = GetOrAdd(smartLogLevel,
                new SmartLogLines());
            smartLogLines.Log<T>(smartLogLevel,
                line);
        }

        public long Size()
        {
            var size = 0L;
            foreach (var smartLogLevel in Keys)
            {
                TryGetValue(smartLogLevel,
                    out var smartLogLines);
                size += smartLogLines?.Count
                        ?? 0;
            }

            return size;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        public void Write(TextWriter textWriter)
        {
            foreach (var smartLogLevel in SmartLogLevelsWriteOrder)
            {
                TryGetValue(smartLogLevel,
                    out var smartLogLines);
                smartLogLines?.Write(textWriter);
            }
        }
    }
}