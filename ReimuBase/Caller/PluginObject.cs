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
            Type[] AllowInterfaces =
            {
                typeof(ITextMessageListener),
                typeof(IOtherMessageReceiver),
                typeof(IMemberJoinLeftListener),
                typeof(IMessageListener),
                typeof(ICommandReceiver),
                typeof(IStartReceiver)
            };
            Type helpMsgType = typeof(IHelpMessage);
            Type clearTimeouteType = typeof(IClearItemsReceiver);
            Type[] types = assembly.GetTypes();
            foreach (Type t in types)
            {
                foreach (Type at in AllowInterfaces)
                    if (at.IsAssignableFrom(t))
                    {
                        object obj = Activator.CreateInstance(t);
                        if (messageListener == null)
                            messageListener = new List<CallablePlugin> {new CallablePlugin(t, obj)};
                        else
                            messageListener.Add(new CallablePlugin(t, obj));
                        break;
                    }

                if (helpMsgType.IsAssignableFrom(t))
                {
                    object obj = Activator.CreateInstance(t);
                    if (helpObjects == null)
                        helpObjects = new List<CallablePlugin> {new CallablePlugin(t, obj)};
                    else
                        helpObjects.Add(new CallablePlugin(t, obj));
                }

                if (clearTimeouteType.IsAssignableFrom(t))
                {
                    object obj = Activator.CreateInstance(t);
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
            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : PluginObject callMesasge(non type) -> " + MethodName);
            if (messageListener == null) return;
            foreach (CallablePlugin plugin in messageListener)
                if (typeof(IMessageListener).IsAssignableFrom(plugin.type))
                    try
                    {
                        if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : PluginObject(forEach0g) -> " + MethodName);
                        CallbackMessage resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
                        GetException(resultobj);
                    }
                    catch (NotImplementedException)
                    {
                    }
        }

        internal void callMessage(string MethodName, object[] parameters, Type type)
        {
            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : PluginObject callMesasge -> " + MethodName);
            if (messageListener == null) return;
            Type allmsgreciver = typeof(IOtherMessageReceiver);
            Type messagelistener = typeof(IMessageListener);
            object[] processAllInterfacesParamters = {parameters[0], parameters[1]};
            foreach (CallablePlugin plugin in messageListener)
            {
                if (allmsgreciver.IsAssignableFrom(plugin.type))
                {
                    // 判断当前循环中的类是不是处理所有消息的接口
                    if (allmsgreciver.IsAssignableFrom(type))
                        try
                        {
                            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : PluginObject(forEach1) -> " + MethodName);
                            CallbackMessage resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
                            GetException(resultobj);
                        }
                        catch (NotImplementedException)
                        {
                        }
                    else
                        try
                        {
                            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : ReceiveAllNormalMessage");
                            CallbackMessage resultobj = (CallbackMessage) plugin.callPlugin("ReceiveAllNormalMessage",
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
                        if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : PluginObject(forEach2) -> " + MethodName);
                        CallbackMessage resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
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
                        if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : PluginObject(forEach3) -> " + MethodName);
                        CallbackMessage resultobj = (CallbackMessage) plugin.callPlugin(MethodName, parameters);
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
            foreach (CallablePlugin plugin in clearTimeoutListener)
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
            string helpmsg = "";
            foreach (CallablePlugin plugin in helpObjects)
                try
                {
                    if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : GetHelpMessage");
                    object helpobj = plugin.callPlugin("GetHelpMessage", new object[] {RawMessage, MessageType});
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
            if (RAPI.getIsDebugEnv()) Console.WriteLine("Message Caller : CallablePlugin callPlugin -> " + MethodName);
            MethodInfo[] methodInfoList = type.GetMethods();
            MethodInfo methodInfo = null;
            foreach (MethodInfo method in methodInfoList)
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