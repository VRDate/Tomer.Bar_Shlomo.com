using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    public class SmartLoggerPerType<T> : ConcurrentDictionary<Type, SmartLogPerTypeByLevel<T>>, 
        IDisposable
    {
        private string Name { get; }

        public SmartLoggerPerType(string name)
        {
            Name = GetName(name);
        }

        private static string GetName(string name)
        {
            string newName = $"{typeof(T)}.{name}";
            return newName;
        }


        private FileStream _fileStream;

        private FileStream FileStream => _fileStream
                                         ?? (_fileStream = CreateFileStream());

        private FileStream CreateFileStream()
        {
            string tempPath = Path.GetTempPath();
            string filepath = Path.Combine(tempPath, $"{Name}.log");
            string line = SmartLogLine.GetLine("Logging",
                "to",
                filepath);
            SmartLogLine smartLogLine = SmartLogLine.Create(typeof(T),
                SmartLogLevel.Trace,
                line);
            Console.Out.WriteLine(smartLogLine.ToString());
            Console.Out.Flush();
            FileStream fileStream = new FileStream(filepath,
                FileMode.Append,
                FileAccess.Write);
            return fileStream;
        }

        private TextWriter _textWriter;

        private TextWriter Logger => _textWriter
                                         ?? (_textWriter = CreateLogger(FileStream));

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        private StreamWriter CreateLogger(FileStream fileStream)
        {
            _fileStream = fileStream;
            StreamWriter streamWriter = new StreamWriter(fileStream);
            return streamWriter;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }


        public void Write()
        {
            foreach (Type type in Keys)
            {
                TryGetValue(type,
                    out SmartLogPerTypeByLevel<T> smartLogByLevel);
                smartLogByLevel?.Write(Logger);
                Logger.WriteLine();
                Logger.Flush();
            }
        }

        private void ReleaseUnmanagedResources()
        {
            Logger.Flush();
            FileStream.Close();
        }

        ~SmartLoggerPerType()
        {
            ReleaseUnmanagedResources();
        }
    }
}