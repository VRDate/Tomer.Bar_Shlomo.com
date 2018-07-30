using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    public class SmartLogLine : IEqualityComparer<SmartLogLine>
    {
        private const int HashKey = 951753;

        private const string ValueSeperator = "\t";
        private const string DateTimeFormat = "O";

        // ReSharper disable once MemberCanBePrivate.Global
        public SmartLogLine(Type type,
            SmartLogLevel smartLogLevel,
            string line)
        {
            At = DateTime.UtcNow;
            MachineName = Environment.MachineName;
            ThreadName = Thread.CurrentThread.Name;
            Type = type;
            Level = smartLogLevel;
            Line = line;
        }

        public DateTime At { get; }

        public string ThreadName { get; }

        public string MachineName { get; }

        public Type Type { get; }

        public SmartLogLevel Level { get; }

        public string Line { get; }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public bool Equals(SmartLogLine smartLogLine1,
            SmartLogLine smartLogLine2)
        {
            int hashCode1 = GetHashCode(smartLogLine1);
            int hashCode2 = GetHashCode(smartLogLine2);
            bool equality = hashCode1
                .Equals(hashCode2);
            return equality;
        }


        public int GetHashCode(SmartLogLine smartLogLine)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            int hashCode = smartLogLine == null
                ? 0
                : smartLogLine.GetHashCode();
            return hashCode;
        }

        public static SmartLogLine Create(Type type,
            SmartLogLevel smartLogLevel,
            string line)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            SmartLogLine smartLogLine = new SmartLogLine(type,
                smartLogLevel,
                line);
            return smartLogLine;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public override int GetHashCode()
        {
            int hashCode = HashKey +
                           At.GetHashCode() +
                           MachineName.GetHashCode() +
                           ThreadName.GetHashCode() +
                           Type.GetHashCode() +
                           Level.Priority() +
                           Line.GetHashCode();
            return hashCode;
        }

        public void Write(TextWriter textWriter)
        {
            string line = ToString();
            textWriter.WriteLine(line);
        }

        public override string ToString()
        {
            string line = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                ValueSeperator,
                At.ToString(DateTimeFormat),
                MachineName,
                ThreadName,
                Type,
                Level.ToLine(),
                Line);
            return line;
        }

        public static string GetLine(string action,
            object obj,
            object message)
        {
            return $"{action} {obj} \"{message}\"";
        }
    }
}