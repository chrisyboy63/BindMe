using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindMe.Commands
{
    public class CommandService
    {
        public static ICommand GetCommand(string pCommand)
        {
            ICommand com = null;
            string commandLine = pCommand.TrimStart();
            string[] commandArgs = commandLine.Split(" ");
            string command = commandArgs[0];

            switch (command.ToUpper())
            {
                case "PROCESS":
                    ProcessCommand pc = new ProcessCommand();
                    com = pc;
                    break;
                case "SOUNDMETER":
                    SoundMeterCommand sc = new SoundMeterCommand();
                    sc.Args = from a in commandArgs where a != commandArgs.First() select a;
                    com = sc;
                    break;
                case "SWAPKEY":
                    SwapKeyCommand sk = new SwapKeyCommand();
                    sk.Args = from a in commandArgs where a != commandArgs.First() select a;
                    com = sk;
                    break;
                default:
                    ProcessCommand pcd = new ProcessCommand();
                    pcd.Args = new string[] { commandLine };
                    com = pcd;
                    break;
            }

            return com;
        }
    }
}
