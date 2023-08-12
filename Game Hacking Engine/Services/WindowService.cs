using Avalonia.Controls;
using Avalonia.Platform;
using Game_Hacking_Engine.ViewModels;
using Game_Hacking_Engine.Views;

namespace Game_Hacking_Engine.Services
{
    internal class WindowService
    {
        public const string ROOT_PATH = @"C:\";
        public static Window? OpenFileDialog()
        {
            FileDialogWindow fileDialogWindow = new()
            {
                DataContext = new FileDialogWindowViewModel()
            };

            if (WindowManager.AddWindow(fileDialogWindow, Windows.FileDialog))
            {
                fileDialogWindow.Initialize();
                return fileDialogWindow;
            }

            return default;
        }


        public static Window? OpenMessageDialog(string title, string message)
        {
            MsgBox msgBoxWindow = new()
            {
                DataContext = new MsgBoxViewModel()
            };

            if (WindowManager.AddWindow(msgBoxWindow, Windows.MsgBox))
            {
                msgBoxWindow.Initialize(title, message);
                return msgBoxWindow;
            }

            return default;
        }

        public static bool IsMaximized(WindowBase window)
        {
            Screen? screen = window.Screens.ScreenFromWindow(window);
            if (screen != null)
            {
                var workingArea = screen.WorkingArea;
                return window.Height >= workingArea.Height && window.Width >= workingArea.Width;
            }

            return false;
        }
    }
}
