using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Game_Hacking_Engine.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using FileDialog = Game_Hacking_Engine.Services.FileDialog;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public class DataGridPEItem
        {
            public DataGridPEItem(string member, uint offset, uint size, uint value)
            {
                Member = member;
                Offset = offset;
                Size = size;
                Value = value;
            }

            public string Member { get; set; }
            public uint Offset { get; set; }
            public uint Size { get; set; }
            public uint Value { get; set; }
        }

        [ObservableProperty]
        private string? fileName;
        [ObservableProperty]
        private string? fileLength;
        [ObservableProperty]
        private string? fileArchitecture;
        [ObservableProperty]
        private string? baseOfImage;
        [ObservableProperty]
        private string? sizeOfImage;

        [ObservableProperty]
        private int selectedTabIndex;
        [ObservableProperty]
        private bool tabOneIsVisible = true; // Default Tab
        [ObservableProperty]
        private bool tabTwoIsVisible;
        [ObservableProperty]
        private bool tabThreeIsVisible;

        [ObservableProperty]
        private ObservableCollection<DataGridPEItem> dosHeaderItems = new();

        public MainWindowViewModel()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            FileName = $"Name: {"None"}";
            FileLength = $"Length: {0} bytes";
            FileArchitecture = $"Architecture: {"Unk"}";
            BaseOfImage = $"Image Base: {0x00000000:X}";
            SizeOfImage = $"Size Of Image: {0} bytes";
            // Default Values
            FileDialog.OnFileSelected += GetFileInfo;
        }

        private void InitializeDataGrid()
        {

        }

        private void GetFileInfo(string path)
        {
            PE_HEADER PE_HEADER = PE.GetPEHeader(path);
            IMAGE_DOS_HEADER IMAGE_DOS_HEADER = PE_HEADER.IMAGE_DOS_HEADER;
            IMAGE_NT_HEADERS IMAGE_NT_HEADERS = PE_HEADER.IMAGE_NT_HEADERS;
            // Parse the informations to the view.
            FileInfo fileInfo = new(path);
            if (fileInfo.Exists)
            {
                FileName = $"Name: {fileInfo.Name}";
                FileLength = $"Length: {fileInfo.Length} bytes";
                FileArchitecture = $"Architecture: {PE.GetArchitectureOfPEHeader(IMAGE_NT_HEADERS.OptionalHeader.Magic)}";
                BaseOfImage = $"Image Base: {IMAGE_NT_HEADERS.OptionalHeader.ImageBase:X}";
                SizeOfImage = $"Size Of Image: {IMAGE_NT_HEADERS.OptionalHeader.SizeOfImage} bytes";
                // Fill datagrid with selected PE HEADER!
                InitializeDataGrid();
            }
        }

        public void OpenFileDialog()
        {
            Window? window = WindowService.OpenFileDialog();
            window?.ShowDialog(WindowManager.GetParent());
        }

        public void OpenOptionsMenu()
        {
            Window? window = WindowService.OpenMessageDialog("Sistema", "eai mano tudaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            window?.ShowDialog(WindowManager.GetParent());
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            // Hack....
            if (e.PropertyName == nameof(SelectedTabIndex))
            {
                TabOneIsVisible = (SelectedTabIndex == 0);
                TabTwoIsVisible = (SelectedTabIndex == 1);
                TabThreeIsVisible = (SelectedTabIndex == 2);
            }
        }
    }
}