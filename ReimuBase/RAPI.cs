using System;
using System.Collections.Generic;
using ReimuAPI.ReimuBase.Caller;
using ReimuAPI.ReimuBase.TgData;

namespace ReimuAPI.ReimuBase
{
    public class RAPI
    {
        public static ExceptionListener GetExceptionListener()
        {
            if (TempData.exceptionListener == null)
            {
                ExceptionListener el = new ExceptionListener();
                TempData.exceptionListener = el;
                return el;
            }

            return TempData.exceptionListener;
        }

        public static void loadPlugins(ExceptionListener exceptionListener = null)
        {
            string[] importantPlugins = new ConfigManager().getConfig().plugins.important;
            string[] normalPlugins = new ConfigManager().getConfig().plugins.normal;
            List<PluginObject> pluginsList = new List<PluginObject>();
            foreach (string i in importantPlugins)
            {
                string pluginsBaseDir = AppDomain.CurrentDomain.BaseDirectory + "plugins\\";
                try
                {
                    PluginObject pluginObject = new PluginObject(pluginsBaseDir + i + ".dll", true, i);
                    pluginsList.Add(pluginObject);
                    Log.i("Plugin \"" + i + "\" (important) load success");
                }
                catch (Exception e)
                {
                    GetExceptionListener().OnException(e);
                }
            }

            foreach (string i in normalPlugins)
            {
                string pluginsBaseDir = AppDomain.CurrentDomain.BaseDirectory + "plugins\\";
                try
                {
                    PluginObject pluginObject = new PluginObject(pluginsBaseDir + i + ".dll", false, i);
                    pluginsList.Add(pluginObject);
                    Log.i("Plugin \"" + i + "\" (normal) load success");
                }
                catch (Exception e)
                {
                    GetExceptionListener().OnException(e);
                }
            }

            TempData.pluginsList = pluginsList;
        }

        public static string getHelpContent(TgMessage message)
        {
            return PluginsCaller.getHelpMessage(TempData.pluginsList, message, message.chat.type);
        }

        public static bool reloadConfig()
        {
            ReimuConfig config = new ConfigManager().reloadConfig();
            return true;
        }

        public static string escapeMarkdown(string text)
        {
            text = text.Replace(@"\", @"\\");
            text = text.Replace("*", @"\*");
            text = text.Replace("_", @"\_");
            text = text.Replace("[", @"\[");
            text = text.Replace("]", @"\]");
            text = text.Replace("`", @"\`");
            return text;
        }
        
        public static bool getIsBlockGroup(int GroupID)
        {
            ReimuConfig config = new ConfigManager().getConfig();
            foreach (int i in config.blockgroup_list)
                if (i == GroupID)
                    return true;
            return false;
        }

        public static bool getIsInWhitelist(int UserID)
        {
            if (getIsBotOP(UserID)) return true;
            if (getIsBotAdmin(UserID)) return true;
            ReimuConfig config = new ConfigManager().getConfig();
            foreach (int i in config.whitelist)
                if (i == UserID)
                    return true;
            return false;
        }

        public static bool getIsBotAdmin(int UserID)
        {
            ReimuConfig config = new ConfigManager().getConfig();
            foreach (int i in config.admin_list)
                if (i == UserID)
                    return true;
            return false;
        }

        public static bool getIsBotOP(long UserID)
        {
            ReimuConfig config = new ConfigManager().getConfig();
            foreach (int i in config.op_list)
                if (i == UserID)
                    return true;
            return false;
        }
    }
}