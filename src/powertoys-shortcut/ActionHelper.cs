using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

/**
 * Action Encoding
 * 
 * url::{Insert URL Here}
 * cmd::{Insert Command Here}
 * clip::{Clipboard Content}
 */

namespace powertoys.shortcut
{
    public class ActionHelper
    {
        Main parent;

        public ActionHelper(Main parent)
        {
            this.parent = parent;
        }

        public void execute(string action)
        {
            string[] split = action.Split("::");
            string type = split[0].ToLower().Trim();
            string arg = split[1].ToLower().Trim();

            if (type == "url")
            {
                Process.Start(new ProcessStartInfo(arg)
                {
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
            } else if (type == "file")
            {
                Process.Start(new ProcessStartInfo(arg)
                {
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
            } else if (type == "cmd")
            {
                Process.Start(new ProcessStartInfo(@"C:\Windows\System32\cmd.exe")
                {
                    Arguments = arg,
                    UseShellExecute = true,
                    CreateNoWindow = false
                });
            }
            else if (type == "start")
            {
                Process.Start(new ProcessStartInfo(split[1].Trim())
                {
                    Arguments = split[2].Trim(),
                    UseShellExecute = false,
                    CreateNoWindow = false
                });
            }
            else if (type == "startasadmin")
            {
                Process.Start(new ProcessStartInfo(split[1].Trim())
                {
                    Arguments = split[2].Trim(),
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    Verb = "runas"
                });
            }
            else if (type == "clip")
            {
                Clipboard.SetText(arg);
            }
        }
    }
}
