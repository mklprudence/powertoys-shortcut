using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Wox.Plugin;

namespace powertoys.shortcut
{
    public class Main : IPlugin, ISettingProvider
    {
        private PluginInitContext _context;

        // Settings Val
        private bool _test;

        public string Name => "Shortcut";

        public string Description => "Customized Shortcut and Actions";

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

        public void Init(PluginInitContext context)
        {
            _context = context;
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public List<Result> Query(Query query)
        {
            var search = query.Search;
            
            return new List<Result>
            {
                new Result
                {
                    Title = search + _test,
                    IcoPath = IconPath,
                    Action = e =>
                    {
                        Clipboard.SetText(search);
                        return true;
                    },
                }
            };
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

        public System.Windows.Controls.Control CreateSettingPanel()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            var test = false;

            if (settings != null && settings.AdditionalOptions != null)
            {
                test = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "Test")?.Value ?? test;
            }

            _test = test;
        }
    }
}
