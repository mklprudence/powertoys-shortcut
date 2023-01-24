using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Wox.Plugin;

namespace powertoys.shortcut
{
    public class ResultHelper
    {
        Main parent;

        public ResultHelper(Main parent)
        {
            this.parent = parent;
        }
        
        public List<ShortcutResult> interpret(string query)
        {
            var config = parent._configHelper.config;

            var split = query.Split(" ", 2);
            var key = split[0];
            var keyreg = new Regex(".*" + Regex.Replace(key, ".{1}", @"$0.*"));

            bool keyExist = config.Shortcuts.ContainsKey(key);
            var otherfits = config.Shortcuts.Where(entry => keyreg.IsMatch(entry.Key) && entry.Key != key);

            List<ShortcutResult> res = new List<ShortcutResult>();
            if (keyExist) res.Add(generateResult(key, query));
            foreach (var entry in otherfits) res.Add(generateResult(entry.Key, query));

            return res;
        }

        private ShortcutResult generateResult(string key, string query)
        {
            var config = parent._configHelper.config;

            var split = query.Split(" ", 2);
            var arg = (split.Length > 1) ? split[1] : "";
            var args = arg.Split(" ");

            var action = insertparams(config.Shortcuts[key].Action, arg, args);
            var desc = insertparams(config.Shortcuts[key].Description, arg, args);

            return new ShortcutResult(key, desc, action, parent);
        }

        private string insertparams(string template, string arg, string[] args)
        {
            // $0 : All
            template = template.Replace(@"$0", arg);

            // $i : Single
            template = Regex.Replace(template, @"\$(\d*)", m => replaceargs(m, args));
            return template;
        }

        private string replaceargs(Match m, string[] args)
        {
            int index = Int32.Parse(m.Groups[1].Value);
            if (index > args.Length) return m.Groups[0].Value;
            return args[index - 1];
        }

        private enum FlagType
        {
            Single, Range, Plus
        }
    }

    public class ShortcutResult
    {
        Main parent;
        string key, desc, action;

        public ShortcutResult(string key, string desc, string action, Main parent)
        {
            this.key = key;
            this.desc = desc;
            this.action = action;
            this.parent = parent;
        }

        public Result toResult()
        {
            return new Result
            {
                Title = desc,
                SubTitle = string.Format("[{0}] {1}", key, parent._configHelper.config.Shortcuts[key].Title),
                IcoPath = parent.IconPath,
                Action = e =>
                {
                    parent._actionHelper.execute(action);
                    return true;
                },
            };
        }
    }
}
