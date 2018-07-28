using System;
using System.Diagnostics.CodeAnalysis;
using Tomer.Bar_Shlomo.com.Logging.Model;

namespace Tomer.Bar_Shlomo.com.Logging
{
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    public static class SmartLoggerFactory
    {
        private static readonly Array SmartLogLevelValues = Enum.GetValues(typeof(SmartLogLevel));

        public static SmartLogPerTypeByLevel<T> Log<T>(this SmartLoggerPerType<T> smartLoggerPerType,
            string action,
            SmartLogLevel smartLogLevel,
            string message)
        {
            var line = SmartLogLine.GetLine(action,
                smartLogLevel.ToLine(),
                message);
            var smartLogPerTypeByLevel = smartLoggerPerType.Log(smartLogLevel,
                line);
            return smartLogPerTypeByLevel;
        }

        public static SmartLogPerTypeByLevel<T> Log<T>(this SmartLoggerPerType<T> smartLoggerPerType,
            SmartLogLevel smartLogLevel,
            string line)
        {
            var smartLogPerTypeByLevel = smartLoggerPerType.GetOrAdd(typeof(T),
                new SmartLogPerTypeByLevel<T>());
            smartLogPerTypeByLevel.Log(smartLogLevel,
                line);
            return smartLogPerTypeByLevel;
        }

        public static SmartLogLine CreateSmartLogLine<T>(this SmartLogLevel smartLogLevel,
            string line)
        {
            SmartLogLine smartLogLine = SmartLogLine.Create(typeof(T),
                smartLogLevel,
                line);
            return smartLogLine;
        }

        public static int Priority(this SmartLogLevel smartLogLevel)
        {
            var index = Index(smartLogLevel);
            var priority = (int) SmartLogLevelValues.GetValue(index);
            return priority;
        }

        private static int Index(this SmartLogLevel smartLogLevel)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            var indexOf = Array.IndexOf(SmartLogLevelValues,
                smartLogLevel);
            return indexOf;
        }

        public static string ToLine(this SmartLogLevel smartLogLevel)
        {
            var name = Enum.GetName(typeof(SmartLogLevel),
                // ReSharper disable once HeapView.BoxingAllocation
                smartLogLevel);
            var priority = smartLogLevel.Priority();
            var line = $"{priority.ToString()}-{name}";
            return line;
        }
    }
}