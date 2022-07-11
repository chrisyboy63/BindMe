using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMeterLib
{
    public class SoundMeterClient
    {
        static SoundMeterClient _client;
        private SoundMeterClient()
        {

        }

        public static SoundMeterClient Instance
        {
            get
            {
                if (_client == null)
                {
                    _client = new SoundMeterClient();
                }

                return _client;
            }
        }

        public void SendCommand(SoundMeterCommands command, params object[] args)
        {
            using (NamedPipeClientStream c = new NamedPipeClientStream(".", SharedConstants.NAMEDPIPENAME, PipeDirection.Out))
            {
                if (!c.IsConnected)
                {
                    c.Connect(1000);
                }

                BinaryWriter bw = new BinaryWriter(c);
                switch (command)
                {
                    case SoundMeterCommands.VolUp:
                    case SoundMeterCommands.VolDown:
                        int vol = Convert.ToInt32(args[0]);
                        bw.Write(new byte[] { (byte)command, (byte)vol });
                        break;
                }

                bw.Flush();
                c.Close();
            }
        }

        public enum SoundMeterCommands
        {
            VolUp = 0x01,
            VolDown = 0x02,
            MuteToggle = 0x03
        }
    }
}
