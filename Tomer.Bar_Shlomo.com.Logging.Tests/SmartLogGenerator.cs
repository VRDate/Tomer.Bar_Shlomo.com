using System;
using System.Threading;
using NUnit.Framework;
using Tomer.Bar_Shlomo.com.Logging.Model;

namespace Tomer.Bar_Shlomo.com.Logging.Tests
{
    public class SmartLogGenerator : IDisposable
    {
        private const string Getting = "Getting";
        private const string Creating = "Creating";
        private const string SmartLogGeneratorName = "Smart Log Generator";
        private const string Size = "Size";

        public static readonly SmartLoggerPerType<SmartLogGenerator> SmartLoggerPerType =
            new SmartLoggerPerType<SmartLogGenerator>();

        private static readonly SmartLogPerTypeByLevel<SmartLogGenerator> ExpectedSmartLogPerTypeByLevelCritical =
            SmartLoggerPerType.Log(Creating,
                SmartLogLevel.Critical,
                SmartLogGeneratorName);

        private static readonly SmartLogPerTypeByLevel<SmartLogGenerator> ExpectedSmartLogPerTypeByLevelError =
            SmartLoggerPerType.Log(Creating,
                SmartLogLevel.Error,
                SmartLogGeneratorName);

        private static readonly SmartLogPerTypeByLevel<SmartLogGenerator> ExpectedSmartLogPerTypeByLevelWarn =
            SmartLoggerPerType.Log(Creating,
                SmartLogLevel.Warn,
                SmartLogGeneratorName);

        private static readonly SmartLogPerTypeByLevel<SmartLogGenerator> ExpectedSmartLogPerTypeByLevelInfo =
            SmartLoggerPerType.Log(Creating,
                SmartLogLevel.Info,
                SmartLogGeneratorName);

        private static readonly SmartLogPerTypeByLevel<SmartLogGenerator> ExpectedSmartLogPerTypeByLevelDebug =
            SmartLoggerPerType.Log(Creating,
                SmartLogLevel.Debug,
                SmartLogGeneratorName);

        private static readonly SmartLogPerTypeByLevel<SmartLogGenerator> ExpectedSmartLogPerTypeByLevelTrace =
            SmartLoggerPerType.Log(Creating,
                SmartLogLevel.Trace,
                SmartLogGeneratorName);

        public SmartLogGenerator(string name,
            ManualResetEvent manualResetEvent)
        {
            Name = name;
            ManualResetEvent = manualResetEvent;
        }

        public string Name { get; set; }
        public ManualResetEvent ManualResetEvent { get; set; }

        public void Dispose()
        {
            Write();
            ManualResetEvent.Dispose();
        }

        public void ThreadCallback()
        {
            SmartLoggerPerType.Log(SmartLogLevel.Trace,
                SmartLogLine.GetLine("Starting",
                    SmartLogGeneratorName,
                    ToString()));
            AssertGetSmartLog(SmartLogLevel.Trace,
                ExpectedSmartLogPerTypeByLevelTrace,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Debug,
                ExpectedSmartLogPerTypeByLevelDebug,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Warn,
                ExpectedSmartLogPerTypeByLevelWarn,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Info,
                ExpectedSmartLogPerTypeByLevelInfo,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Error,
                ExpectedSmartLogPerTypeByLevelError,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Critical,
                ExpectedSmartLogPerTypeByLevelCritical,
                SmartLogLevel.Debug);
            SmartLoggerPerType.Log(SmartLogLevel.Trace,
                SmartLogLine.GetLine("Complited",
                    SmartLogGeneratorName,
                    ToString()));
            Write();
            ManualResetEvent.Set();
        }


        private static void AssertGetSmartLog(SmartLogLevel smartLogLevel,
            SmartLogPerTypeByLevel<SmartLogGenerator> expectedSmartLogPerTypeByLevel,
            SmartLogLevel logLevel)
        {
            var smartLogPerTypeByLevel = AssertGetSmartLog(Getting,
                smartLogLevel,
                expectedSmartLogPerTypeByLevel);
            Thread.Sleep(100);
            var smartLogLines = LogSize(smartLogLevel,
                smartLogPerTypeByLevel);
            var line = SmartLogLine.GetLine(logLevel.ToLine(),
                SmartLogGeneratorName,
                smartLogLevel.ToLine());
            smartLogPerTypeByLevel.Log(logLevel,
                line);
            Thread.Sleep(100);
        }

        private static SmartLogPerTypeByLevel<SmartLogGenerator> AssertGetSmartLog(string action,
            SmartLogLevel smartLogLevel,
            SmartLogPerTypeByLevel<SmartLogGenerator> expectedSmartLogPerTypeByLevel)
        {
            var smartLog = SmartLoggerPerType.Log(action,
                smartLogLevel,
                SmartLogGeneratorName);
            Assert.NotNull(expectedSmartLogPerTypeByLevel);
            Assert.NotNull(smartLog);
            Assert.AreEqual(expectedSmartLogPerTypeByLevel,
                smartLog);
            LogSize(smartLogLevel,
                smartLog);
            return smartLog;
        }

        private static SmartLogLines LogSize(SmartLogLevel smartLogLevel,
            SmartLogPerTypeByLevel<SmartLogGenerator> smartLogPerTypeByLevel)
        {
            smartLogPerTypeByLevel.TryGetValue(smartLogLevel,
                out var smartLogLines);
            Assert.NotNull(smartLogLines);
            var line = SmartLogLine.GetLine(SmartLogGeneratorName,
                Size,
                smartLogPerTypeByLevel.Size().ToString());
            smartLogPerTypeByLevel.Log(smartLogLevel,
                line);
            Thread.Sleep(100);
            return smartLogLines;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        private static void Write()
        {
            LogSize(SmartLogLevel.Debug,
                ExpectedSmartLogPerTypeByLevelDebug);
            SmartLoggerPerType.Write();
        }
    }
}