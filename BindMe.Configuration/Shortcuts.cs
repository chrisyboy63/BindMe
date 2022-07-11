using System;
using System.Collections.Generic;

using BindMe.Commands;
using BindMe.Constants;

namespace BindMe.Configuration
{
    public class ShortcutsRoot
    {
        public List<Shortcut> Shortcuts = new List<Shortcut>();
    }

    public class Shortcut {
        public Modifiers Modifier;
        public Keys Key;
        public ICommand Command;
    }
}
