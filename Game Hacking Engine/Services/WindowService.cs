using Avalonia.Controls;
using Game_Hacking_Engine.ViewModels;
using Game_Hacking_Engine.Views;

namespace Game_Hacking_Engine.Services
{
    internal class WindowService
    {
        public const string ROOT_PATH = @"C:\";
        public static Window OpenFileDialog()
        {
            FileDialogWindow fileDialogWindow = new()
            {
                DataContext = new FileDialogWindowViewModel()
            };

            fileDialogWindow.Initialize();
            return fileDialogWindow;
        }
    }
}
