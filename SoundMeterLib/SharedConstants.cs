using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMeterLib
{
    public static class SharedConstants
    {
        public const byte VOLUP = 0x01;
        public const byte VOLDOWN = 0x02;
        public const byte MUTETOGGLE = 0x03;

        public const string NAMEDPIPENAME = "sound_meter";
    }
}
