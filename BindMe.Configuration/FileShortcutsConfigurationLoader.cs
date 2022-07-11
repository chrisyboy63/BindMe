using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using BindMe.Constants;
using Microsoft.Extensions.Logging;

namespace BindMe.Configuration {
    public class FileShortcutsConfigurationLoader : IShortcutsConfigurationLoader
    {
        private readonly ILogger _log;

        public FileShortcutsConfigurationLoader(ILogger log)
        {
            _log = log;
        }
        public ShortcutsRoot LoadConfig()
        {
            ShortcutsRoot scr = new ShortcutsRoot();
           
            const string configFile = "bindmerc";
            string[] searchPath = {
                                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), configFile), // User profile directory
                                    Path.Combine(Directory.GetCurrentDirectory(), configFile), // Current Directory
                                    Path.Combine(Directory.GetDirectoryRoot(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), configFile), // Processes Directory
                                    };

            foreach (string cf in searchPath)
            {
                if (File.Exists(cf))
                {
                    using (StreamReader sr = new StreamReader(File.OpenRead(cf)))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith(Environment.NewLine) || string.IsNullOrWhiteSpace(line) || line.Contains("#"))
                            {
                                continue;
                            }

                            Shortcut sc = new Shortcut();
                            IEnumerable<string> keys = from q in line.Split("+") select q.Trim();
                            foreach (string key in keys)
                            {
                                Modifiers m; 
                                if (Enum.TryParse(key, true, out m))
                                {
                                    sc.Modifier |= m;
                                }

                                Keys k;
                                if (Enum.TryParse(key, true, out k))
                                {
                                    sc.Key = k;
                                }
                            }
                            line = sr.ReadLine();
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                throw new Exception("Binding has no command with it");
                            }
                            _log.LogInformation($"Building command {sc.Modifier.ToString().Replace(", ", " + ")} + {sc.Key}{Environment.NewLine}\t[{line.Trim()}] from config{Environment.NewLine}\t[{cf}]");
                            sc.Command = Commands.CommandService.GetCommand(line);
                            scr.Shortcuts.Add(sc);
                        }
                    }
                }
            }

            return scr;
        }
    }
}