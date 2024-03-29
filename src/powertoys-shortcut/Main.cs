﻿using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.Win32;
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
        public ConfigHelper _configHelper;
        public ResultHelper _resultHelper;
        public ActionHelper _actionHelper;

        // IPlugin properties
        public string Name => "Shortcut";
        public string Description => "Customized Shortcut and Actions";

        // Settings val
        private bool _test;

        // Result Icon Path
        public string IconPath { get; set; }
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

            // Helpers
            _configHelper = new ConfigHelper(this, _context.CurrentPluginMetadata.PluginDirectory + @"\config.json");
            _resultHelper = new ResultHelper(this);
            _actionHelper = new ActionHelper(this);
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconPath = "images/shortcut.light.png";
            }
            else
            {
                IconPath = "images/shortcut.dark.png";
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
            var res = _resultHelper.interpret(search);

            if (res.Count == 0)
            {
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

            return res.ConvertAll(i => i.toResult());
        }

        // Context Menus
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            throw new NotImplementedException();
        }
    }
}
