using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Game_Hacking_Engine.Services;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class MsgBoxViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? title;
        [ObservableProperty]
        private string? message;

        public void Close()
        {
            Window window = WindowManager.GetWindow(Windows.MsgBox);
            window.Close();
        }

        public bool IsOk { get; private set; }
        public void Ok(bool isOk)
        {
            IsOk = isOk;
            Close();
        }
    }
}
