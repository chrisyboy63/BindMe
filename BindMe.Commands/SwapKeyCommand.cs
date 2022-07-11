using System;
using System.Collections.Generic;
using WinApi;
using BindMe.Constants;
using System.Runtime.InteropServices;

namespace BindMe.Commands
{
    public class SwapKeyCommand : ICommand
    {

        public IEnumerable<string> Args { get; set; }

        public void Execute()
        {
            List<HotKeyApi.Input> inputs = new List<HotKeyApi.Input>();

            string arg = string.Empty;
            var enumerator = Args.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var obj = enumerator.Current;
                obj = obj.Trim();
                if (obj == "+")
                {
                    continue;
                }

                Keys k;
                if (Enum.TryParse(obj, true, out k))
                {
                    inputs.AddRange(GenerateInput(k));
                }
            }
            if (inputs.Count > 0)
            {
                HotKeyApi.SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(HotKeyApi.Input)));
            }
        }

        private HotKeyApi.Input[] GenerateInput(BindMe.Constants.Keys key)
        {
            HotKeyApi.Input[] inputs = new HotKeyApi.Input[2];
            HotKeyApi.InputUnion keyDownU = new HotKeyApi.InputUnion();
            HotKeyApi.KeyboardInput keyDown = new HotKeyApi.KeyboardInput();
            keyDown.wVk = Convert.ToUInt16(key);
            keyDown.dwFlags = Convert.ToUInt32(HotKeyApi.KeyEventF.KeyDown);
            keyDown.dwExtraInfo = WindowsMessaging.GetMessageExtraInfo();
            HotKeyApi.InputUnion keyUpU = new HotKeyApi.InputUnion();
            HotKeyApi.KeyboardInput keyUp = new HotKeyApi.KeyboardInput();
            keyUp.wVk = Convert.ToUInt16(key);
            keyUp.dwFlags = Convert.ToUInt32(HotKeyApi.KeyEventF.KeyUp);
            keyUp.dwExtraInfo = WindowsMessaging.GetMessageExtraInfo();

            keyDownU.ki = keyDown;
            keyUpU.ki = keyUp;

            HotKeyApi.Input kdi = new HotKeyApi.Input() { type = (int)HotKeyApi.InputType.Keyboard };
            HotKeyApi.Input kui = new HotKeyApi.Input() { type = (int)HotKeyApi.InputType.Keyboard };

            kdi.u = keyDownU;
            kui.u = keyUpU;

            inputs[0] = kdi;
            inputs[1] = kui;

            return inputs;
        }

    }
}
