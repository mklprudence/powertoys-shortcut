using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace powertoys.shortcut
{
    public class ConfigHelper
    {
        string filepath;
        Main parent;
        public Config config;

        public ConfigHelper(Main parent, string filepath)
        {
            this.parent = parent;
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
        public Dictionary<string, Shortcut> Shortcuts { get; set; }
    }

    public class Shortcut
    {
        public string Description { get; set; }
        public string Action { get; set; }
    }
}
