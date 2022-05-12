using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using System.Collections.Generic;
using System.Windows.Forms;
using Wox.Plugin;

namespace powertoys.shortcut
{
    public class Main : IPlugin
    {
        private string IconPath { get; set; }

        private PluginInitContext Context { get; set; }
        public string Name => "Shortcut";

        public string Description => "Customized Shortcut and Actions";

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
            Context = context;
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        public List<Result> Query(Query query)
        {
            var search = query.Search;
            
            return new List<Result>
            {
                new Result
                {
                    Title = search,
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
    }
}
