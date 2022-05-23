using System;
using System.Collections.Generic;
using System.Text;

namespace powertoys.shortcut
{
    public class Config
    {
        public Dictionary<string, Shortcut>? Shortcuts { get; set; }
    }

    public class Shortcut
    {
        public string Action { get; set; }
    }
}
