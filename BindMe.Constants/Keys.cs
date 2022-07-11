﻿using System;

namespace BindMe.Constants
{
    public enum Keys
    {
        NUM0 = 0x30,
        NUM1 = 0x31,
        NUM2 = 0x32,
        NUM3 = 0x33,
        NUM4 = 0x34,
        NUM5 = 0x35,
        NUM6 = 0x36,
        NUM7 = 0x37,
        NUM8 = 0x38,
        NUM9 = 0x39,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LEFT = 0x25,
        UP = 0x26,
        RIGHT = 0x27,
        DOWN = 0x28,
        ENTER = 0x0D,
        BTNVolUp = 0xAF,
        BTNVolDown = 0xAE,
        BTNMute = 0xAD,
        BTNPRINTSCREEN = 0x2C,
        BTNSLEEP = 0x5F,
        BTNNTRACK = 0xB0,
        BTNPTRACK = 0xB1,
        BTNPLAYPAUSE = 0xB3
    }

    [Flags]
    public enum Modifiers
    {
        NoModifier = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        Shift = 0x0004,
        Super = 0x0008,
    }
}
