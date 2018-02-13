using System;
using System.Collections.Generic;
using System.Reflection;
using ReimuAPI.ReimuBase.Interfaces;
using ReimuAPI.ReimuBase.TgData;

namespace ReimuAPI.ReimuBase.Caller
{
    public class PluginObject
    {
        public PluginObject(string DllPath, bool IsImportant, string PluginName)
        {
            assembly = Assembly.LoadFrom(DllPath);
            var AllowInterfaces =
            {
                typeof(ITextMessageListener),
                typeof(IOtherMessageReceiver),
                typeof(IMemberJoinLeftListener),
                typeof(IMessageListener),
                typeof(ICommandReceiver),
                typeof(IStartReceiver)
            };
            var helpMsgType = typeof(IHelpMessage);
            var clearTimeouteType = typeof(IClearItemsReceiver);
            var types = assembly.GetTypes();
            foreach (var t in types)
            {
                foreach (var at in AllowInterfaces)
                    if (at.IsAssignableFrom(t))
                    {
                        var obj = Activator.CreateInstance(t);
                        if (messageListener == null)
                            messageListener = new List<CallablePlugin> {new CallablePlugin(t, obj)};
                        else
                            messageListener.Add(new CallablePlugin(t, obj));
                        break;
                    }

                if (helpMsgType.IsAssignableFrom(t))
                {
                    var obj = Activator.CreateInstance(t);
                    if (helpObjects == null)
                        helpObjects = new List<CallablePlugin> {new CallablePlugin(t, obj)};
                    else
                        helpObjects.Add(new CallablePlugin(t, obj));
                }

                if (clearTimeouteType.IsAssignableFrom(t))
                {
                    var obj = Activator.CreateInstance(t);
                    if (clearTimeoutListener == null)
                        clearTimeoutListener = new List<CallablePlugin> {new CallablePlugin(t, obj)};
                    else
                        clearTimeoutListener.Add(new CallablePlugin(t, obj));
                }
            }

            this.IsImportant = IsImportant;
            this.PluginName = PluginName;
        }

        internal Assembly assembly { get; }
        internal List<CallablePlugin> messageListener { get; }
        internal List<CallablePlugin> helpObjects { get; }
        internal List<CallablePlugin> clearTimeoutListener { get; }
        public string PluginName { get; }
        internal bool IsImportant { get; }

        internal void callMessage(string MethodName, object[] parameters)
        {
            if (messageListener == null) return;
            foreach (var plugin in messageListener)
                if (typeof(IMessageListener).IsAssignableFrom(plugin.type))
                    try
                    {
                        var resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
                        GetException(resultobj);
                    }
                    catch (NotImplementedException)
                    {
                    }
        }

        internal void callMessage(string MethodName, object[] parameters, Type type)
        {
            if (messageListener == null) return;
            var allmsgreciver = typeof(IOtherMessageReceiver);
            var messagelistener = typeof(IMessageListener);
            var processAllInterfacesParamters = {parameters[0], parameters[1]};
            foreach (var plugin in messageListener)
            {
                if (allmsgreciver.IsAssignableFrom(plugin.type))
                {
                    // 判断当前循环中的类是不是处理所有消息的接口
                    if (allmsgreciver.IsAssignableFrom(type))
                        try
                        {
                            var resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
                            GetException(resultobj);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    else
                        try
                        {
                            var resultobj = (CallbackMessage) plugin.callPlugin("ReceiveAllNormalMessage",
                                processAllInterfacesParamters);
                            GetException(resultobj);
                        }
                        catch (NotImplementedException)
                        {
                        }

                    continue;
                }

                if (allmsgreciver.IsAssignableFrom(type)) continue;

                if (messagelistener.IsAssignableFrom(plugin.type))
                {
                    // 如果这个类是处理普通消息的接口则运行这个插件
                    try
                    {
                        var resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
                        GetException(resultobj);
                    }
                    catch (NotImplementedException)
                    {
                    }

                    continue;
                }

                if (type.IsAssignableFrom(plugin.type))
                    try
                    {
                        var resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
                        GetException(resultobj);
                    }
                    catch (NotImplementedException)
                    {
                    }
            }
        }

        internal void callClear()
        {
            if (clearTimeoutListener == null) return;
            foreach (var plugin in clearTimeoutListener)
                try
                {
                    plugin.callPlugin("ClearItems", new object[] { });
                }
                catch (NotImplementedException)
                {
                }
        }

        internal string getHelpContent(TgMessage RawMessage, string MessageType)
        {
            if (helpObjects == null) return "";
            var helpmsg = "";
            foreach (var plugin in helpObjects)
                try
                {
                    var helpobj = plugin.callPlugin("GetHelpMessage", new object[] {RawMessage, MessageType});
                    if (helpobj != null) helpmsg += (string) helpobj;
                }
                catch (NotImplementedException)
                {
                }

            return helpmsg;
        }

        internal void GetException(CallbackMessage msg)
        {
            if (msg.StopProcess) throw new StopProcessException();
        }
    }

    public class CallablePlugin
    {
        internal CallablePlugin(Type type, object obj)
        {
            this.type = type;
            this.obj = obj;
        }

        internal Type type { get; }
        internal object obj { get; }

        internal object callPlugin(string MethodName, object[] parameters)
        {
            var methodInfoList = type.GetMethods();
            MethodInfo methodInfo = null;
            foreach (var method in methodInfoList)
                if (method.Name == MethodName)
                    methodInfo = method;
            if (methodInfo == null)
            {
                Log.w("Call plugin failed: Cannot find method: " + MethodName);
                return new CallbackMessage();
            }

            return methodInfo.Invoke(obj, parameters);
        }
    }
}