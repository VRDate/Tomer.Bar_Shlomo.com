using System;
using System.Threading;
using NUnit.Framework;
using Tomer.Bar_Shlomo.com.Logging.Model;

namespace Tomer.Bar_Shlomo.com.Logging.Tests
{
    public class SmartLogGenerator : IDisposable
    {
        private const string Getting = "Getting";
        private const string SmartLogGeneratorName = "Smart Log Generator";
        private const string Size = "Size";

        public SmartLogGenerator(string name,
            ManualResetEvent manualResetEvent,
            SmartLoggerPerType<SmartLoggerTests> SmartLoggerPerType)
        {
            Name = name;
            ManualResetEvent = manualResetEvent;
            this.SmartLoggerPerType = SmartLoggerPerType;
        }

        private SmartLoggerPerType<SmartLoggerTests> SmartLoggerPerType { get; }

        public string Name { get; }

        public ManualResetEvent ManualResetEvent { get; }

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
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Debug,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Warn,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Info,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Error,
                SmartLogLevel.Debug);

            AssertGetSmartLog(SmartLogLevel.Critical,
                SmartLogLevel.Debug);
            SmartLoggerPerType.Log(SmartLogLevel.Trace,
                SmartLogLine.GetLine("Complited",
                    SmartLogGeneratorName,
                    ToString()));
            Write();
            ManualResetEvent.Set();
        }


        private void AssertGetSmartLog(SmartLogLevel smartLogLevel,
            SmartLogLevel logLevel)
        {
            SmartLoggerPerType.TryGetValue(typeof(SmartLoggerTests),
                out SmartLogPerTypeByLevel<SmartLoggerTests> expectedSmartLogPerTypeByLevel);
            SmartLogPerTypeByLevel<SmartLoggerTests> smartLogPerTypeByLevel = AssertGetSmartLog(Getting,
                smartLogLevel,
                expectedSmartLogPerTypeByLevel);
            Thread.Sleep(100);
            LogSize(smartLogLevel);
            string line = SmartLogLine.GetLine(logLevel.ToLine(),
                SmartLogGeneratorName,
                smartLogLevel.ToLine());
            smartLogPerTypeByLevel.Log(logLevel,
                line);
            Thread.Sleep(100);
        }

        private SmartLogPerTypeByLevel<SmartLoggerTests> AssertGetSmartLog(string action,
            SmartLogLevel smartLogLevel,
            SmartLogPerTypeByLevel<SmartLoggerTests> expectedSmartLogPerTypeByLevel)
        {
            Assert.NotNull(expectedSmartLogPerTypeByLevel);
            SmartLogPerTypeByLevel<SmartLoggerTests> smartLogPerTypeByLevel = SmartLoggerPerType.Log(action,
                smartLogLevel,
                SmartLogGeneratorName);
            Assert.NotNull(smartLogPerTypeByLevel);
            Assert.AreEqual(expectedSmartLogPerTypeByLevel,
                smartLogPerTypeByLevel);
            LogSize(smartLogLevel);
            return smartLogPerTypeByLevel;
        }


        public override string ToString()
        {
            return $"{Name}";
        }

        private void Write()
        {
            LogSize(SmartLogLevel.Debug);
            SmartLoggerPerType.Write();
        }

        private void LogSize(SmartLogLevel smartLogLevel)
        {
            SmartLoggerPerType.TryGetValue(typeof(SmartLoggerTests),
                out SmartLogPerTypeByLevel<SmartLoggerTests> smartLogPerTypeByLevel);
            Assert.NotNull(smartLogPerTypeByLevel);
            
            smartLogPerTypeByLevel.TryGetValue(smartLogLevel,
                out SmartLogLines smartLogLines);
            Assert.NotNull(smartLogLines);
            
            string line = SmartLogLine.GetLine(SmartLogGeneratorName,
                Size,
                smartLogPerTypeByLevel.Size().ToString());
            smartLogPerTypeByLevel.Log(smartLogLevel,
                line);
            Assert.IsNotEmpty(smartLogLines);
            Thread.Sleep(100);
        }
    }
}