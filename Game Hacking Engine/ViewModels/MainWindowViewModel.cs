using Avalonia.Controls;
using Game_Hacking_Engine.Services;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public void OpenFile(Window wParent)
        {
            Window window = WindowService.OpenFileDialog();
            window.ShowDialog(wParent);
        }
    }
}