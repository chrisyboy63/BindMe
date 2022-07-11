using System;
using System.Collections.Generic;
using System.Linq;
using SoundMeterLib;

namespace BindMe.Commands
{
    public class SoundMeterCommand : ICommand
    {
        public IEnumerable<string> Args { get; set; }

        public void Execute()
        {
            string command = Args.First();

            if (command.StartsWith("+") || command.StartsWith("-"))
            {
                char volDirection = command[0];
                int percent = Convert.ToInt32(command.Substring(1));
                SoundMeterClient.Instance.SendCommand(command.StartsWith("+") ? SoundMeterClient.SoundMeterCommands.VolUp : SoundMeterClient.SoundMeterCommands.VolDown, percent);
            }
        }

    }
}
