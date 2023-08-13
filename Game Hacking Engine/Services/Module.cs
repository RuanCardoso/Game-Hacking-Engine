using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Game_Hacking_Engine.Services
{
    internal class Module
    {
        public static string? FilePath;
        public static int OffsetMode = 0; // 0 - By architecture, 1 - x86, 2 - x64

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
            return typeCode switch
            {
                TypeCode.Boolean => sizeof(bool) * 2,
                TypeCode.Byte => sizeof(byte) * 2,
                TypeCode.Int16 => sizeof(short) * 2,
                TypeCode.UInt16 => sizeof(ushort) * 2,
                TypeCode.Int32 => sizeof(int) * 2,
                TypeCode.UInt32 => sizeof(uint) * 2,
                TypeCode.Int64 => sizeof(long) * 2,
                TypeCode.UInt64 => sizeof(ulong) * 2,
                TypeCode.Single => sizeof(float) * 2,
                TypeCode.Double => sizeof(double) * 2,
                TypeCode.Decimal => sizeof(decimal) * 2,
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
