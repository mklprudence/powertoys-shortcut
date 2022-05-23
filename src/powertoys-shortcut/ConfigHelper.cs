using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace powertoys.shortcut
{
    public class ConfigHelper
    {
        string filepath;
        public Config config;

        public ConfigHelper(string filepath)
        {
            this.filepath = filepath;
            updateConfig();
        }

        public void updateConfig()
        {
            string filetext = System.IO.File.ReadAllText(filepath);
            config = JsonSerializer.Deserialize<Config>(filetext);
        }
    }

    public class Config
    {
        public Dictionary<string, Shortcut>? Shortcuts { get; set; }
    }

    public class Shortcut
    {
        public string Action { get; set; }
    }
}
