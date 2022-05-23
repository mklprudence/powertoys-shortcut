using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Forms;
using Wox.Plugin;

namespace powertoys.shortcut
{
    public class Main : IPlugin, ISettingProvider, IContextMenu
    {
        private PluginInitContext _context;
        private ConfigHelper _configHelper;

        // IPlugin properties
        public string Name => "Shortcut";
        public string Description => "Customized Shortcut and Actions";

        // Settings val
        private bool _test;

        // Result Icon Path
        private string IconPath { get; set; }
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>()
        {
            new PluginAdditionalOption()
            {
                Key = "Test",
                DisplayLabel = "test",
                Value = false,
            },
        };

        // Init
        public void Init(PluginInitContext context)
        {
            _context = context;
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
            _configHelper = new ConfigHelper(_context.CurrentPluginMetadata.PluginDirectory + @"\config.json");
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconPath = "images/guid.light.png";
            }
            else
            {
                IconPath = "images/guid.dark.png";
            }
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }

        // Settings
        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            var test = false;

            if (settings != null && settings.AdditionalOptions != null)
            {
                test = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "Test")?.Value ?? test;
            }

            _test = test;

            _configHelper.updateConfig();
        }

        public System.Windows.Controls.Control CreateSettingPanel()
        {
            throw new System.NotImplementedException();
        }

        // Query
        public List<Result> Query(Query query)
        {
            Config config = _configHelper.config;
            
            var search = query.Search;
            var key = search.Split(" ")[0];

            if (config?.Shortcuts.ContainsKey(key) == true)
            {
                var action = config?.Shortcuts[key].Action;
                return new List<Result>
                {
                    new Result
                    {
                        Title = action,
                        IcoPath = IconPath,
                        Action = e =>
                        {
                            Clipboard.SetText(action);
                            return true;
                        },
                    }
                };
            }

            return new List<Result>
            {
                new Result
                {
                    Title = "No such shortcut",
                    IcoPath = IconPath,
                    Action = e =>
                    {
                        return true;
                    },
                }
            };
        }

        // Context Menus
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            throw new NotImplementedException();
        }
    }
}
