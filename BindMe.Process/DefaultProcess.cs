using System;
using Microsoft.Extensions.Logging;

using BindMe.Constants;
using BindMe.Configuration;

namespace BindMe.Process
{
    public class DefaultProcess : IDisposable
    {
        private ILogger _log;
        private static HotKeyObserver _observer;
        private IShortcutsConfigurationLoader configLoader;

        public DefaultProcess(ILogger log) {
            _log = log;
            _observer = new HotKeyObserver(_log);
            configLoader = new FileShortcutsConfigurationLoader(_log);
            log.LogInformation("Starting up default process.");
        }

        public void init() {

            var shortcuts = configLoader.LoadConfig();
            foreach (var shortcut in shortcuts.Shortcuts)
            {
                _observer.RegisterHotKey(new HotKey(shortcut.Modifier, shortcut.Key, () => shortcut.Command.Execute(), _log));
            }
            _observer.RunLoop();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
