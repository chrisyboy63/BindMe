using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SoundMeterLib;

namespace SoundMeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex myMutex;

        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);


        protected override void OnStartup(StartupEventArgs e)
        {
            bool aIsNewInstance = false;
            myMutex = new Mutex(true, "SoundMeter", out aIsNewInstance);
            if (!aIsNewInstance)
            {
                SoundMeterClient.SoundMeterCommands command;
                var args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                {
                    string commandArgs = args[1].ToUpper().Trim('-');

                    switch (commandArgs)
                    {
                        case "VOLUP":
                            int volNum = Convert.ToInt32(args[2]);
                            command = SoundMeterClient.SoundMeterCommands.VolUp;
                            SoundMeterClient.Instance.SendCommand(command, volNum);
                            break;
                        case "VOLDOWN":
                            int volDownNum = Convert.ToInt32(args[2]);
                            command = SoundMeterClient.SoundMeterCommands.VolDown;
                            SoundMeterClient.Instance.SendCommand(command, volDownNum);
                            break;
                    }

                }

                Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }
}
