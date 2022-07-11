using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WinApi;

using BindMe.Constants;
using Microsoft.Extensions.Logging;

namespace BindMe.Process
{
    public interface IHotKey
    {
        void Invoke(UIntPtr hotKeyId);
    }
    public class HotKey
    {
        readonly Modifiers _modifiers;
        readonly Keys _key;
        internal int _id = -1;
        private Action _callback;
        private ILogger _log;

        public Modifiers ModifierKeys
        {
            get { return _modifiers; }
        }

        public Keys Key
        {
            get { return _key; }
        }

        internal HotKey(Modifiers modifiers, Keys key, Action callback, ILogger log)
        {
            _log = log;
            _modifiers = modifiers;
            _key = key;
            _callback = callback;
        }

        internal void RegisterKey(int id)
        {
            _id = id;
        }

        internal void Notify(int id)
        {
            if (id == _id)
            {
                _log.LogInformation($"{_modifiers} + {_key} - Has been invoked");
                _callback();
            }
        }
    }

    public class HotKeyObserver: IDisposable
    {
        List<HotKey> _hotKeys = new List<HotKey>();
        int idRef = 0;
        Thread msgLoop;
        ILogger _log;

        public HotKeyObserver(ILogger log)
        {
            _log = log;
            GenerateLoop();
        }

        private void MessageLoop()
        {
            WindowsMessaging.MSG msg;
            int ret;

            foreach(HotKey hKey in _hotKeys)
            {
                bool hKeySuccess = HotKeyApi.RegisterHotKey(IntPtr.Zero, hKey._id, (int)hKey.ModifierKeys, (int)hKey.Key);
                if (!hKeySuccess)
                {
                    // throw new Exception("Failed to Register Hot Key");
                    _log.LogError($"Failed to register [{hKey.ModifierKeys.ToString().Replace(", ", " + ")} + {hKey.Key}]");
                }
            }

            while ((ret = WindowsMessaging.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0)
            {
                if (msg.message == WindowsMessaging.WM_HOTKEY)
                {
                    NotifyHotKeyObserver((int)msg.wParam);
                }
            }
        }

        public void RegisterHotKey(HotKey hotKey)
        {
            System.Threading.Interlocked.Increment(ref idRef);
            hotKey.RegisterKey(idRef);
            _hotKeys.Add(hotKey);
        }

        private void GenerateLoop()
        {
            msgLoop = new Thread(MessageLoop);
        }

        public void RunLoop()
        {
            msgLoop.Start();
        }

        public void NotifyHotKeyObserver(int hotKeyId)
        {
            foreach (HotKey hKey in _hotKeys)
            {
                hKey.Notify(hotKeyId);
            }
        }

        public void UnRegisterHotKey(HotKey hKey)
        {
            HotKeyApi.UnregisterHotKey(IntPtr.Zero, hKey._id);
            _hotKeys.Remove(hKey);
        }

        public void Dispose()
        {
            //foreach (HotKey hKey in _hotKeys)
            //{
            //    HotKeyApi.UnregisterHotKey(IntPtr.Zero, hKey._id);
            //}
            _hotKeys.Clear();
        }
    }
}
