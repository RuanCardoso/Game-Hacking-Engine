using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Game_Hacking_Engine.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Architecture = Game_Hacking_Engine.Services.Architecture;
using FileDialog = Game_Hacking_Engine.Services.FileDialog;
using Module = Game_Hacking_Engine.Services.Module;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public class DGItem
        {
            public string? Member { get; set; }
            public string? Offset { get; set; }
            public string? Size { get; set; }
            public string? Value { get; set; }
            public string? Comment { get; set; }
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
        private ObservableCollection<DGItem>? dosHeaderItems;
        [ObservableProperty]
        private ObservableCollection<DGItem>? ntHeadersItems;
        [ObservableProperty]
        private ObservableCollection<DGItem>? fileHeadersItems;

        public MainWindowViewModel()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            FileDialog.OnFileSelected += GetFileInfo;
            // Default Values
            FileName = $"Name: {"None"}";
            FileLength = $"Length: {0} bytes";
            FileArchitecture = $"Architecture: {"Unk"}";
            BaseOfImage = $"Image Base: 0x{0}";
            SizeOfImage = $"Size Of Image: {0} bytes";
        }

        private ObservableCollection<DGItem> CreateDgCollection<T>(T instance)
        {
            ObservableCollection<DGItem> collection = new();
            if (instance != null)
            {
                FieldInfo[] fieldInfos = Reflection.GetFields<T>();
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    var fieldInfo = fieldInfos[i];
                    DGItem pEItem = new()
                    {
                        Member = fieldInfo.Name,
                        Offset = Reflection.GetOffsetOfField<T>(fieldInfo.Name).ToString(),
                        Value = Reflection.GetFieldValueToString(fieldInfo, instance),
                        Size = Reflection.GetTypeCodeToString(fieldInfo),
                    };
                    collection.Add(pEItem);
                }
            }
            return collection;
        }

        private void InitializeDataGrid(IMAGE_DOS_HEADER DOS_HEADER, IMAGE_NT_HEADERS NT_HEADERS, Architecture architecture)
        {
            var collection = CreateDgCollection(DOS_HEADER);
            DosHeaderItems = collection;
        }

        private void GetFileInfo(string path) // Fire when module is opened!
        {
            PE_HEADER PE_HEADER = PE.GetPEHeader(path);
            IMAGE_DOS_HEADER IMAGE_DOS_HEADER = PE_HEADER.IMAGE_DOS_HEADER;
            IMAGE_NT_HEADERS IMAGE_NT_HEADERS = PE_HEADER.IMAGE_NT_HEADERS;
            // Parse the informations to the view.
            FileInfo fileInfo = new(path);
            if (fileInfo.Exists)
            {
                Architecture architecture = PE.GetArchitectureOfPEHeader(IMAGE_NT_HEADERS.OptionalHeader.Magic);
                if (architecture != Architecture.Unk)
                {
                    FileName = $"Name: {fileInfo.Name}";
                    FileLength = $"Length: {fileInfo.Length} bytes";
                    FileArchitecture = $"Architecture: {architecture}";
                    BaseOfImage = $"Image Base: {IMAGE_NT_HEADERS.OptionalHeader.ImageBase.ToString(Module.GetStringFmt(architecture))}";
                    SizeOfImage = $"Size Of Image: {IMAGE_NT_HEADERS.OptionalHeader.SizeOfImage} bytes";
                    // Fill datagrid with selected PE HEADER!
                    InitializeDataGrid(IMAGE_DOS_HEADER, IMAGE_NT_HEADERS, architecture);
                }
            }
        }

        public async void ChangeOffsetMode(int mode)
        {
            if (Module.FilePath != null)
            {
                if (await WindowService.ShowMessageAsync("Changing the addressing mode for offsets (x86-x64) requires reanalysis of the selected module."))
                {
                    Module.OffsetMode = mode;
                    FileDialog.Select(Module.FilePath);
                }
                else
                {
                    WindowService.ShowMessage("Tip: Consider changing the addressing mode of offsets before selecting the module to prevent reanalysis. This can save you time and streamline your workflow. If you choose to cancel, remember this suggestion for future reference. Your efficiency is our priority! 😊");
                }
            }
            else
            {
                Module.OffsetMode = mode;
            }
        }

        public void ChangeTheme(bool isDarkMode)
        {
            Module.ChangeTheme(isDarkMode);
        }

        public void OpenFileDialog()
        {
            Window? window = WindowService.OpenFileDialog();
            window?.ShowDialog(WindowManager.GetParent());
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedTabIndex))
            {
                TabOneIsVisible = (SelectedTabIndex == 0);
                TabTwoIsVisible = (SelectedTabIndex == 1);
                TabThreeIsVisible = (SelectedTabIndex == 2);
            }
        }
    }
}