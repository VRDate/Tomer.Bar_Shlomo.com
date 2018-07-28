﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace Tomer.Bar_Shlomo.com.Logging.Model
{
    public class SmartLogLine : IEqualityComparer<SmartLogLine>
    {
        private const int HashKey = 951753;

        private const string VlaueSeperator = "\t";

        // ReSharper disable once MemberCanBePrivate.Global
        public SmartLogLine(Type type,
            SmartLogLevel smartLogLevel,
            string line)
        {
            At = DateTime.UtcNow;
            ThreadName = Thread.CurrentThread.Name;
            Type = type;
            Level = smartLogLevel;
            Line = line;
        }

        public DateTime At { get; }

        public string ThreadName { get; }

        public Type Type { get; }

        public SmartLogLevel Level { get; }

        public string Line { get; }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public bool Equals(SmartLogLine smartLogLine1,
            SmartLogLine smartLogLine2)
        {
            var hashCode1 = GetHashCode(smartLogLine1);
            var hashCode2 = GetHashCode(smartLogLine2);
            var equality = hashCode1
                .Equals(hashCode2);
            return equality;
        }


        public int GetHashCode(SmartLogLine smartLogLine)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var hashCode = smartLogLine == null
                ? 0
                : smartLogLine.GetHashCode();
            return hashCode;
        }

        public static SmartLogLine Create(Type type,
            SmartLogLevel smartLogLevel,
            string line)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            var smartLogLine = new SmartLogLine(type,
                smartLogLevel,
                line);
            return smartLogLine;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public override int GetHashCode()
        {
            var hashCode = HashKey +
                           At.GetHashCode() +
                           ThreadName.GetHashCode() +
                           Type.GetHashCode() +
                           Level.Priority() +
                           Line.GetHashCode();
            return hashCode;
        }

        public void Write(TextWriter textWriter)
        {
            var line = ToString();
            textWriter.WriteLine(line);
        }

        public override string ToString()
        {
            var line = string.Format("{1:O}{0}{2}{0}{3}{0}{4}{0}{5}",
                VlaueSeperator,
                // ReSharper disable once HeapView.BoxingAllocation
                At,
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
            // ReSharper disable once HeapView.BoxingAllocation
            return $"{action} {obj} \"{message}\"";
        }
    }
}