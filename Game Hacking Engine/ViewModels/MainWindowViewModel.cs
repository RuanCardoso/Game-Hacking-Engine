using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Game_Hacking_Engine.Services;
using System.IO;
using FileDialog = Game_Hacking_Engine.Services.FileDialog;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? fileName;
        [ObservableProperty]
        private string? fileLength;
        [ObservableProperty]
        private string? fileArchitecture;

        public MainWindowViewModel()
        {
            FileDialog.OnFileSelected += FileDialog_OnFileSelected;
        }

        private void FileDialog_OnFileSelected(string path)
        {
            PE_HEADER PE_HEADER = PE.GetPEHeader(path);
            FileInfo fileInfo = new(path);
            if (fileInfo.Exists)
            {
                FileName = $"Name: {fileInfo.Name}";
                FileLength = $"Length: {fileInfo.Length} bytes";
                FileArchitecture = $"Architecture: {"x86"}";
            }
        }

        public void OpenFileDialog(Window wParent)
        {
            Window? window = WindowService.OpenFileDialog();
            window?.ShowDialog(wParent);
        }
    }
}