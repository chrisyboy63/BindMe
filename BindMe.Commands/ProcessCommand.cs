using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace BindMe.Commands
{
    public class ProcessCommand : ICommand
    {
        public IEnumerable<string> Args { get; set; }

        public void Execute()
        {
            Process p = new Process();
            p.StartInfo.FileName = "CMD.EXE";
            p.StartInfo.Arguments = "/C " + Args.First();

            p.Start();
        }
    }
}
