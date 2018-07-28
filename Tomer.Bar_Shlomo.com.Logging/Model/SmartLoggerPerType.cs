using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    public class SmartLoggerPerType<T> : ConcurrentDictionary<Type, SmartLogPerTypeByLevel<T>>, IDisposable
    {
        private FileStream _fileStream;

        private TextWriter _textWriter;

        private FileStream FileStream => _fileStream
                                         ?? (_fileStream = CreateFileStream());

        public TextWriter TextWriter => _textWriter
                                        ?? (_textWriter = CreateLog(FileStream));

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        private StreamWriter CreateLog(FileStream fileStream)
        {
            _fileStream = fileStream;
            var streamWriter = new StreamWriter(fileStream);
            return streamWriter;
        }

        private FileStream CreateFileStream()
        {
            var tempPath = Path.GetTempPath();
            var filepath = Path.Combine(tempPath, $"{typeof(T)}.log");
            var line = SmartLogLine.GetLine("Logging",
                "to",
                filepath);
            var smartLogLine = SmartLogLine.Create(typeof(T),
                SmartLogLevel.Trace,
                line);
            Console.Out.WriteLine(smartLogLine.ToString());
            Console.Out.Flush();
            var fileStream = new FileStream(filepath,
                FileMode.Append,
                FileAccess.Write);
            return fileStream;
        }

        public void Write()
        {
            foreach (var type in Keys)
            {
                TryGetValue(type,
                    out var smartLogByLevel);
                smartLogByLevel?.Write(TextWriter);
                TextWriter.WriteLine();
                TextWriter.Flush();
            }
        }

        private void ReleaseUnmanagedResources()
        {
            TextWriter.Flush();
            FileStream.Close();
        }

        ~SmartLoggerPerType()
        {
            ReleaseUnmanagedResources();
        }
    }
}