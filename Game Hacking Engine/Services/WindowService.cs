using Avalonia.Controls;
using Avalonia.Platform;
using Game_Hacking_Engine.ViewModels;
using Game_Hacking_Engine.Views;
using System.Threading.Tasks;

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

        public static void ShowMessage(string message, string title = "System")
        {
            Window? window = OpenMessageDialog(title, message);
            window?.ShowDialog(WindowManager.GetParent());
        }

        public static Task<bool> ShowMessageAsync(string message, string title = "System")
        {
            TaskCompletionSource<bool> taskCompletionSource = new();
            Window? window = OpenMessageDialog(title, message);
            window!.Closed += (_, _) => taskCompletionSource.SetResult(((MsgBox)window).GetResult());
            window!.ShowDialog(WindowManager.GetParent());
            return taskCompletionSource.Task;
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
