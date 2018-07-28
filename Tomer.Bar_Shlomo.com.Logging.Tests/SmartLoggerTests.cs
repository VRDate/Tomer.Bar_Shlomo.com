using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using NUnit.Framework;
using Tomer.Bar_Shlomo.com.Logging.Model;
//Logging to C:\Users\[UserName]\AppData\Local\Temp\Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log
// ReSharper disable HeapView.BoxingAllocation
namespace Tomer.Bar_Shlomo.com.Logging.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public partial class SmartLoggerTests
    {
        private const int MaxThreads = 64;
        private static Thread[] Threads = new Thread[MaxThreads];
        private static ManualResetEvent[] ManualResetEvents = new ManualResetEvent[MaxThreads];
        private static SmartLogGenerator[] SmartLogGeneratorsArray = new SmartLogGenerator[MaxThreads];

        public SmartLoggerTests()
        {
            SmartLogGenerator.SmartLoggerPerType.Log(SmartLogLevel.Trace,
                SmartLogLine.GetLine("Starting",
                    "SmartLogger",
                    "Tests"));
        }


        [Test]
        public void CurrentThreadTest()
        {
            SmartLogGenerator smartLogGenerator = CreateSmartLogGenerator("SmartLoggerTests.CurrentThreadTest[1]");
            Assert.NotNull(smartLogGenerator);
            smartLogGenerator.ThreadCallback();
            smartLogGenerator.Dispose();
        }

        [Test]
        public void MultiThreadTest()
        {
            Threads = new Thread[MaxThreads];
            ManualResetEvents = new ManualResetEvent[MaxThreads];
            SmartLogGeneratorsArray = new SmartLogGenerator[MaxThreads];
            for (int threadIndex = 0; threadIndex < MaxThreads; threadIndex++)
            {
                Threads[threadIndex] = CreateThread($"SmartLoggerTests.MultiThreadTest[{threadIndex + 1}]",
                    out ManualResetEvents[threadIndex],
                    SmartLogGeneratorsArray[threadIndex]);
                Threads[threadIndex].Start();
                Thread.Sleep(200);
            }

            // ReSharper disable once CoVariantArrayConversion
            WaitHandle.WaitAll(ManualResetEvents);

            for (int threadIndex = 0; threadIndex < MaxThreads; threadIndex++)
            {
                SmartLogGenerator smartLogGenerators = SmartLogGeneratorsArray[threadIndex];
                smartLogGenerators?.Dispose();
            }
        }

        private static Thread CreateThread(string smartLoggerTestName,
            out ManualResetEvent manualResetEvent,
            SmartLogGenerator smartLogGenerator)
        {
            smartLogGenerator = CreateSmartLogGenerator(smartLoggerTestName);
            manualResetEvent = smartLogGenerator.ManualResetEvent;
            Thread thread = new Thread(() =>
                smartLogGenerator.ThreadCallback());
            thread.Name = smartLoggerTestName;
            return thread;
        }

        private static SmartLogGenerator CreateSmartLogGenerator(string smartLoggerTestName)
        {
            SmartLogGenerator smartLogGenerator = new SmartLogGenerator(smartLoggerTestName,
                new ManualResetEvent(false));
            return smartLogGenerator;
        }
    }
}