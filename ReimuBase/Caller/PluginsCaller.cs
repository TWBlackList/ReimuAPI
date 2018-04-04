using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ReimuAPI.ReimuBase.Interfaces;
using ReimuAPI.ReimuBase.TgData;

namespace ReimuAPI.ReimuBase.Caller
{
    internal class PluginsCaller
    {
        internal static void callPlugins(List<PluginObject> plugins, string method, string JsonMessage,
            object[] objects = null)
        {
            foreach (var pl in plugins)
                if (pl.IsImportant)
                    try
                    {
                        pl.callMessage(method, objects);
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (StopProcessException)
                    {
                        return;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                        throw e;
                    }
                else
                    new Task(() =>
                    {
                        try
                        {
                            pl.callMessage(method, objects);
                        }
                        catch (NotImplementedException)
                        {
                        }
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                            throw e;
                        }
                    }).Start();
        }

        internal static void callTextReceiver(List<PluginObject> plugins, string method, string JsonMessage,
            object[] objects = null)
        {
            var validType = typeof(ITextMessageListener);
            foreach (var pl in plugins)
                if (pl.IsImportant)
                    try
                    {
                        pl.callMessage(method, objects, validType);
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (StopProcessException)
                    {
                        return;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                        throw e;
                    }
                else
                    new Task(() =>
                    {
                        try
                        {
                            pl.callMessage(method, objects, validType);
                        }
                        catch (NotImplementedException)
                        {
                        }
                        catch (StopProcessException)
                        {
                        }
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                            throw e;
                        }
                    }).Start();
        }

        internal static void callCommandReceiver(List<PluginObject> plugins, string method, string JsonMessage,
            object[] objects = null)
        {
            var validType = typeof(ICommandReceiver);
            foreach (var pl in plugins)
                if (pl.IsImportant)
                    try
                    {
                        pl.callMessage(method, objects, validType);
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (StopProcessException)
                    {
                        return;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                        throw e;
                    }
                else
                    new Task(() =>
                    {
                        try
                        {
                            pl.callMessage(method, objects, validType);
                        }
                        catch (NotImplementedException)
                        {
                        }
                        catch (StopProcessException)
                        {
                        }
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                            throw e;
                        }
                    }).Start();
        }

        internal static void callStartReceiver(List<PluginObject> plugins, string method, string JsonMessage,
            object[] objects = null)
        {
            var validType = typeof(ICommandReceiver);
            foreach (var pl in plugins)
                if (pl.IsImportant)
                    try
                    {
                        pl.callMessage(method, objects, validType);
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (StopProcessException)
                    {
                        return;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                        throw e;
                    }
                else
                    new Task(() =>
                    {
                        try
                        {
                            pl.callMessage(method, objects, validType);
                        }
                        catch (NotImplementedException)
                        {
                        }
                        catch (StopProcessException)
                        {
                        }
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                            throw e;
                        }
                    }).Start();
        }

        internal static void callMemberJoinReceiver(List<PluginObject> plugins, string method, string JsonMessage,
            object[] objects = null)
        {
            var validType = typeof(IMemberJoinLeftListener);
            foreach (var pl in plugins)
                if (pl.IsImportant)
                    try
                    {
                        pl.callMessage(method, objects, validType);
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (StopProcessException)
                    {
                        return;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                        throw e;
                    }
                else
                    new Task(() =>
                    {
                        try
                        {
                            pl.callMessage(method, objects, validType);
                        }
                        catch (NotImplementedException)
                        {
                        }
                        catch (StopProcessException)
                        {
                        }
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                            throw e;
                        }
                    }).Start();
        }

        internal static void callOtherMessageReceiver(List<PluginObject> plugins, string method, string JsonMessage,
            object[] objects = null)
        {
            var validType = typeof(IMemberJoinLeftListener);
            foreach (var pl in plugins)
                if (pl.IsImportant)
                    try
                    {
                        pl.callMessage(method, objects, validType);
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (StopProcessException)
                    {
                        return;
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                        throw e;
                    }
                    catch (Exception e)
                    {
                        RAPI.GetExceptionListener().OnException(e, JsonMessage);
                    }
                else
                    new Task(() =>
                    {
                        try
                        {
                            pl.callMessage(method, objects, validType);
                        }
                        catch (NotImplementedException)
                        {
                        }
                        catch (StopProcessException)
                        {
                        }
                        catch (TargetInvocationException e)
                        {
                            if (e.InnerException.GetType().IsAssignableFrom(typeof(StopProcessException))) return;
                            throw e;
                        }
                    }).Start();
        }

        internal static string getHelpMessage(List<PluginObject> plugins, TgMessage RawMessage, string MessageType)
        {
            var msg = "";
            foreach (var pl in plugins)
            {
                string pluginhelpmsg = null;
                try
                {
                    pluginhelpmsg = pl.getHelpContent(RawMessage, MessageType);
                }
                catch (NotImplementedException)
                {
                }
                catch (Exception e)
                {
                    RAPI.GetExceptionListener().OnException(e);
                }

                if (pluginhelpmsg != null || pluginhelpmsg != "")
                    msg += "Plugin: " + pl.PluginName + "\n" + pluginhelpmsg + "\n\n";
            }

            return msg;
        }
    }
}