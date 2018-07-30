using System.Diagnostics.CodeAnalysis;
using System.Threading;
using NUnit.Framework;
using Tomer.Bar_Shlomo.com.Logging.Model;

/*Logging to 
 Windows
 C:\Users\[UserName]\AppData\Local\Temp\Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log
 Linux
 /tmp/Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log
 */
// ReSharper disable HeapView.BoxingAllocation
namespace Tomer.Bar_Shlomo.com.Logging.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public class SmartLoggerTests
    {
        private const int MaxThreads = 64;
        private static Thread[] Threads = new Thread[MaxThreads];
        private static ManualResetEvent[] ManualResetEvents = new ManualResetEvent[MaxThreads];
        private static SmartLogGenerator[] SmartLogGeneratorsArray = new SmartLogGenerator[MaxThreads];


        [Test]
        public void CurrentThreadTest()
        {
            string smartLoggerPerTypeName = GetSmartLoggerTestName("CurrentThreadTest",
                0);
            SmartLoggerPerType<SmartLoggerTests> smartLoggerPerType = CreateSmartLoggerPerType(smartLoggerPerTypeName);
            SmartLogGenerator smartLogGenerator = CreateSmartLogGenerator(smartLoggerPerTypeName,
                smartLoggerPerType);
            Assert.NotNull(smartLogGenerator);
            smartLogGenerator.ThreadCallback();
            smartLogGenerator.Dispose();
        }

        [Test]
        public void MultiThreadsSingleLogTest()
        {
            Threads = new Thread[MaxThreads];
            ManualResetEvents = new ManualResetEvent[MaxThreads];
            SmartLogGeneratorsArray = new SmartLogGenerator[MaxThreads];
            string smartLoggerPerTypeName = GetSmartLoggerTestName("MultiThreadsSingleLogTest",
                0);
            SmartLoggerPerType<SmartLoggerTests> smartLoggerPerType = CreateSmartLoggerPerType(smartLoggerPerTypeName);
            for (int threadIndex = 0; threadIndex < MaxThreads; threadIndex++)
            {
                string smartLoggerTestName = GetSmartLoggerTestName("MultiThreadsSingleLogTest",
                    threadIndex);
                SmartLogGeneratorsArray[threadIndex] = CreateSmartLogGenerator(smartLoggerTestName,
                    smartLoggerPerType);
                ManualResetEvents[threadIndex] = SmartLogGeneratorsArray[threadIndex].ManualResetEvent;
                Threads[threadIndex] = CreateThread(SmartLogGeneratorsArray[threadIndex]);
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

        [Test]
        public void MultiThreadsMultiLogsTest()
        {
            Threads = new Thread[MaxThreads];
            ManualResetEvents = new ManualResetEvent[MaxThreads];
            SmartLogGeneratorsArray = new SmartLogGenerator[MaxThreads];
            for (int threadIndex = 0; threadIndex < MaxThreads; threadIndex++)
            {
                string smartLoggerTestName = GetSmartLoggerTestName("MultiThreadsMultiLogsTest",
                    threadIndex);
                SmartLoggerPerType<SmartLoggerTests> smartLoggerPerType = CreateSmartLoggerPerType(smartLoggerTestName);
                SmartLogGeneratorsArray[threadIndex] = CreateSmartLogGenerator(smartLoggerTestName,
                    smartLoggerPerType);
                ManualResetEvents[threadIndex] = SmartLogGeneratorsArray[threadIndex].ManualResetEvent;
                Threads[threadIndex] = CreateThread(SmartLogGeneratorsArray[threadIndex]);
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

        private static string GetSmartLoggerTestName(string testName,
            int threadIndex)
        {
            return $"{testName}[{threadIndex + 1}]";
        }

        private static SmartLoggerPerType<SmartLoggerTests> CreateSmartLoggerPerType(string smartLoggerPerTypeName)
        {
            return new SmartLoggerPerType<SmartLoggerTests>(smartLoggerPerTypeName);
        }

        private static SmartLogGenerator CreateSmartLogGenerator(string smartLoggerTestName,
            SmartLoggerPerType<SmartLoggerTests> smartLoggerPerType)
        {
            SmartLogGenerator smartLogGenerator = new SmartLogGenerator(smartLoggerTestName,
                new ManualResetEvent(false),
                smartLoggerPerType);
            return smartLogGenerator;
        }

        private static Thread CreateThread(SmartLogGenerator smartLogGenerator)
        {
            Thread thread = new Thread(smartLogGenerator.ThreadCallback);
            thread.Name = smartLogGenerator.Name;
            return thread;
        }
    }
}