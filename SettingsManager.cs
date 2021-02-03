using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace WBot2
{
    static class SettingsManager
    {
        public static Config Cfg;
        private static string ConfigFile = Directory.GetCurrentDirectory() + "\\config.json";
        private static string OldSuffix = ".old";

        public static void LoadSettings()
        {
            if (File.Exists(ConfigFile))
            {
                string json = File.ReadAllText(ConfigFile);
                Cfg = JsonConvert.DeserializeObject<Config>(json);
            }
            else
            {
                Console.WriteLine("No config found. Creating default.");
                Console.WriteLine("Enter bot access token");
                string token = Console.ReadLine();
                Cfg = new Config
                {
                    Token = token,
                };
            }
            SaveSettings();
        }

        public static void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(Cfg);
            if (File.Exists(ConfigFile))
            {
                if (File.Exists(ConfigFile + OldSuffix))
                    File.Delete(ConfigFile + OldSuffix);
                File.Move(ConfigFile, ConfigFile + OldSuffix);
            }
            File.WriteAllText(ConfigFile, json);
        }
    }
    public class Config
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class ServerSetting
    {
        public ulong AnnounceChannelId { get; set; }
        public ulong FMasterChannelId { get; set; }
    }
}
