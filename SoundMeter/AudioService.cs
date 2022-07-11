using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;

namespace SoundMeter
{
    public class AudioService
    {

        public int GetVolume()
        {
            MMDevice dev = GetDefaultDevice();
            int vol = Convert.ToInt32(dev.AudioEndpointVolume.MasterVolumeLevelScalar * 100);

            return vol;
        }

        public int SetVolume(int newVol)
        {
            newVol = newVol > 100 ? 100 : newVol;
            newVol = newVol < 0 ? 0 : newVol;
            MMDevice dev = GetDefaultDevice();
            dev.AudioEndpointVolume.MasterVolumeLevelScalar = ((float)newVol / 100.0f);
            return newVol;
        }

        private MMDevice GetDefaultDevice()
        {
            MMDeviceEnumerator devEmum = new MMDeviceEnumerator();
            var dev = devEmum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            return dev;
        }
    }
}
