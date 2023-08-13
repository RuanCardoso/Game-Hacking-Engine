using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Game_Hacking_Engine.Services
{
    internal class Module
    {
        public static string? FilePath;
        public static int OffsetMode = 0; // 0 - By architecture, 1 - x86, 2 - x64 // 0(Default) - By File Arch

        public static string GetStringFmtByOffsetMode(Architecture architecture)
        {
            return OffsetMode switch
            {
                0 => GetStringFmt(architecture),
                1 => GetStringFmt(Architecture.x86),
                2 => GetStringFmt(Architecture.x64),
                _ => GetStringFmt(Architecture.x86),
            };
        }

        public static string GetStringFmt(Architecture architecture)
        {
            return architecture switch
            {
                Architecture.x86 => "X8",
                Architecture.x64 => "X16",
                _ => "X8",
            };
        }

        public static string GetStringFmt(TypeCode typeCode)
        {
            return $"X{GetHexDigits(typeCode)}";
        }

        public static uint GetHexDigits(TypeCode typeCode)
        {
            uint baseOfBit = 2U;
            return typeCode switch
            {
                TypeCode.Boolean => sizeof(bool) * baseOfBit,
                TypeCode.Byte => sizeof(byte) * baseOfBit,
                TypeCode.Int16 => sizeof(short) * baseOfBit,
                TypeCode.UInt16 => sizeof(ushort) * baseOfBit,
                TypeCode.Int32 => sizeof(int) * baseOfBit,
                TypeCode.UInt32 => sizeof(uint) * baseOfBit,
                TypeCode.Int64 => sizeof(long) * baseOfBit,
                TypeCode.UInt64 => sizeof(ulong) * baseOfBit,
                TypeCode.Single => sizeof(float) * baseOfBit,
                TypeCode.Double => sizeof(double) * baseOfBit,
                TypeCode.Decimal => sizeof(decimal) * baseOfBit,
                _ => sizeof(uint)
            };
        }

        public static void ChangeTheme(bool isDarkMode)
        {
            WindowManager.SetTheme(isDarkMode); // Changes the theme of windows that have not yet been opened.
            Window[] windows = WindowManager.GetWindows();
            for (int i = 0; i < windows.Length; i++)
            {
                // Changes the theme of windows that are already open.
                Window window = windows[i];
                window.RequestedThemeVariant = isDarkMode ? ThemeVariant.Dark : ThemeVariant.Light;
            }
        }
    }
}