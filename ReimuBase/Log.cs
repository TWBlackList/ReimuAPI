using System;
using System.Diagnostics;

namespace ReimuAPI.ReimuBase
{
    public static class Log
    {
        public static void i(string content)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();
            Console.WriteLine("[INFO] [" + methodBase.DeclaringType.FullName + "] " + content);
        }

        public static void w(string content)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();
            Console.WriteLine("[WARN] [" + methodBase.DeclaringType.FullName + "] " + content);
        }

        public static void e(string content)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();
            Console.WriteLine("[ERROR] [" + methodBase.DeclaringType.FullName + "] " + content);
        }
    }
}