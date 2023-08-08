using Avalonia.Controls;
using System.Collections.Generic;

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
        public static Dictionary<Windows, Window> windows = new();

        public static bool AddWindow(Window window, Windows wKey)
        {
            window.Closed += (_, _) => GetWindow(wKey);
            return windows.TryAdd(wKey, window);
        }

        public static Window GetWindow(Windows wKey)
        {
            windows.Remove(wKey, out Window? window);
            return window!;
        }
    }
}
