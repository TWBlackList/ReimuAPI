﻿using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReimuAPI.ReimuBase
{
    public class ConfigManager
    {
        public ReimuConfig getConfig()
        {
            if (TempData.reimuConfig == null)
            {
                string configPath = Environment.GetEnvironmentVariable("BOT_CONFIGPATH");
                if (configPath == "" || configPath == null) configPath = "./config.json";
                string json = File.ReadAllText(configPath);
                ReimuConfig data = (ReimuConfig) new DataContractJsonSerializer(
                    typeof(ReimuConfig)
                ).ReadObject(
                    new MemoryStream(
                        Encoding.UTF8.GetBytes(json)
                    )
                );
                TempData.reimuConfig = data;
                return data;
            }

            return TempData.reimuConfig;
        }

        public ReimuConfig reloadConfig()
        {
            TempData.reimuConfig = null;
            if (TempData.reimuConfig == null)
            {
                string configPath = Environment.GetEnvironmentVariable("BOT_CONFIGPATH");
                if (configPath == "" || configPath == null) configPath = "./config.json";
                string json = File.ReadAllText(configPath);
                ReimuConfig data = (ReimuConfig) new DataContractJsonSerializer(
                    typeof(ReimuConfig)
                ).ReadObject(
                    new MemoryStream(
                        Encoding.UTF8.GetBytes(json)
                    )
                );
                TempData.reimuConfig = data;
                return data;
            }

            return TempData.reimuConfig;
        }
    }

    public class ReimuConfig
    {
        public string bind { get; set; }
        public string api_key { get; set; }
        public string api_host { get; set; }
        public long admin_group { get; set; } = 0;
        public bool debug { get; set; } = false;
        public int[] admin_list { get; set; }
        public int[] op_list { get; set; }
        public PluginsListBundle plugins { get; set; }
    }

    public class PluginsListBundle
    {
        public string[] important { get; set; }
        public string[] normal { get; set; }
    }
}