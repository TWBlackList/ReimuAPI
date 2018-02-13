using System;
using System.Diagnostics;

namespace ReimuAPI.ReimuBase
{
    public class ExceptionListener
    {
        private readonly long AdminGroupID;

        public ExceptionListener()
        {
            AdminGroupID = new ConfigManager().getConfig().admin_group;
        }

        public void OnException(Exception exception, string JsonString = null)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();
            var errmsg = "[ERROR] [" + methodBase.DeclaringType.FullName + "] ";
            errmsg += "Error: Have an exception: " + exception;
            if (JsonString != null) errmsg += "\n\nRAW Json: " + JsonString;
            Console.WriteLine(errmsg);
            if (AdminGroupID != 0) TgApi.getDefaultApiConnection().sendMessage(AdminGroupID, errmsg);
        }

        public void OnJsonDecodeError(Exception exception, string JsonString)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();
            var errmsg = "[ERROR] [" + methodBase.DeclaringType.FullName + "] ";
            errmsg += "Error: JSON decode error: " + exception;
            if (JsonString != null) errmsg += "\n\nRAW Json: " + JsonString;
            Console.WriteLine(errmsg);
            if (AdminGroupID != 0) TgApi.getDefaultApiConnection().sendMessage(AdminGroupID, errmsg);
        }
    }
}