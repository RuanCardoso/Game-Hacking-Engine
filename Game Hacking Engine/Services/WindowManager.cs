using Avalonia.Controls;
using Avalonia.Styling;
using System.Collections.Generic;
using System.Linq;

namespace Game_Hacking_Engine.Services
{
    enum Windows
    {
        Main,
        FileDialog,
        MsgBox,
    }

    internal class WindowManager
    {
        private static readonly Dictionary<Windows, Window> windows = new();
        private static bool isDarkMode = true;
        public static bool AddWindow(Window window, Windows wKey)
        {
            window.RequestedThemeVariant = isDarkMode ? ThemeVariant.Dark : ThemeVariant.Light;
            window.Closed += (_, _) => GetWindow(wKey);
            return windows.TryAdd(wKey, window);
        }

        public static Window? GetWindow(Windows wKey)
        {
#pragma warning disable IDE0046 
            if (windows.Remove(wKey, out Window? window))
            {
                return window;
            }
#pragma warning restore IDE0046
            return default;
        }

        public static void SetTheme(bool isDarkMode)
        {
            WindowManager.isDarkMode = isDarkMode;
        }

        public static Window GetParent()
        {
            return GetWindows()[^2];
        }

        public static Window GetLast()
        {
            return GetWindows()[^1];
        }

        public static Window[] GetWindows()
        {
            return windows.Values.ToArray();
        }
    }
}
