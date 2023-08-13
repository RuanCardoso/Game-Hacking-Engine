using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Game_Hacking_Engine.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Architecture = Game_Hacking_Engine.Services.Architecture;
using FileDialog = Game_Hacking_Engine.Services.FileDialog;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public class DataGridPEItem
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
        private ObservableCollection<DataGridPEItem>? dosHeaderItems;
        [ObservableProperty]
        private ObservableCollection<DataGridPEItem>? ntHeadersItems;
        [ObservableProperty]
        private ObservableCollection<DataGridPEItem>? fileHeadersItems;

        public MainWindowViewModel()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            FileName = $"Name: {"None"}";
            FileLength = $"Length: {0} bytes";
            FileArchitecture = $"Architecture: {"Unk"}";
            BaseOfImage = $"Image Base: 0x{0}";
            SizeOfImage = $"Size Of Image: {0} bytes";
            // Default Values
            FileDialog.OnFileSelected += GetFileInfo;
        }

        private void InitializeDataGrid(IMAGE_DOS_HEADER DOS_HEADER, IMAGE_NT_HEADERS NT_HEADERS, Architecture architecture)
        {
            DosHeaderItems = new()
            {
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_magic), DOS_HEADER.e_magic, architecture, "indicating an MS-DOS executable file in the file's header structure"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_cblp), DOS_HEADER.e_cblp, architecture, "the number of bytes in the last page of a program segment in an MS-DOS executable file"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_cp), DOS_HEADER.e_cp, architecture, "the count of relocation entries stored in the header of an MS-DOS executable file"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_crlc), DOS_HEADER.e_crlc, architecture, "the count of relocation entries in the header of an MS-DOS executable file"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_cparhdr), DOS_HEADER.e_cparhdr, architecture, "the number of paragraphs in the header of an MS-DOS executable file, which is typically set to 4"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_minalloc), DOS_HEADER.e_minalloc, architecture, "the minimum number of paragraphs of additional memory required by an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_maxalloc), DOS_HEADER.e_maxalloc, architecture, "the maximum number of paragraphs of additional memory required by an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_ss), DOS_HEADER.e_ss, architecture, "the initial value of the stack segment register for an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_sp), DOS_HEADER.e_sp, architecture, "the initial value of the stack pointer register for an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_csum), DOS_HEADER.e_csum, architecture, "the checksum value for an MS-DOS executable file, used for error-checking purposes"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_ip), DOS_HEADER.e_ip, architecture, "the initial value of the instruction pointer register for an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_cs), DOS_HEADER.e_cs, architecture, "the initial value of the code segment register for an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_lfarlc), DOS_HEADER.e_lfarlc, architecture, "the file address of the relocation table for an MS-DOS executable program"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_ovno), DOS_HEADER.e_ovno, architecture, "the overlay number for an MS-DOS executable file"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_res), DOS_HEADER.e_res[0], architecture, "used for different purposes, such as padding or future expansion, in MS-DOS executable files", $"{nameof(IMAGE_DOS_HEADER.e_res)}[{DOS_HEADER.e_res.Length}]"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_oemid), DOS_HEADER.e_oemid, architecture, "OEM identifier, which is used to identify the manufacturer of the executable file"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_oeminfo), DOS_HEADER.e_oeminfo, architecture, "additional information related to the OEM (Original Equipment Manufacturer)"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, ushort>(nameof(IMAGE_DOS_HEADER.e_res2), DOS_HEADER.e_res2[0], architecture, "reserved bytes used for various purposes, such as future expansion or alignment", $"{nameof(IMAGE_DOS_HEADER.e_res2)}[{DOS_HEADER.e_res2.Length}]"),
                CreateDataGridPEItem<IMAGE_DOS_HEADER, uint>(nameof(IMAGE_DOS_HEADER.e_lfanew), DOS_HEADER.e_lfanew, architecture, "DOS headers that indicates the file offset of the PE (Portable Executable) header", $"{nameof(IMAGE_DOS_HEADER.e_lfanew)} -> Nt Headers")
            };

            uint pos = DOS_HEADER.e_lfanew;
            NtHeadersItems = new()
            {
                CreateDataGridPEItem<IMAGE_NT_HEADERS, uint>(nameof(IMAGE_NT_HEADERS.Signature), NT_HEADERS.Signature, architecture, "a specific signature value to indicate the presence of a valid PE header.", size:pos),
                CreateDataGridPEItem<IMAGE_NT_HEADERS, uint>(nameof(IMAGE_NT_HEADERS.FileHeader), default, architecture, "a structure that contains information about the characteristics and layout of the executable file", size:pos),
                CreateDataGridPEItem<IMAGE_NT_HEADERS, uint>(nameof(IMAGE_NT_HEADERS.OptionalHeader), default, architecture, "a structure that contains additional information about the executable", size:pos),
            };

            pos = NT_HEADERS.FileHeader.Machine;
            FileHeadersItems = new()
            {
                CreateDataGridPEItem<IMAGE_FILE_HEADER, ushort>(nameof(IMAGE_FILE_HEADER.Machine), NT_HEADERS.FileHeader.Machine, architecture, "", size:pos),
                CreateDataGridPEItem<IMAGE_FILE_HEADER, ushort>(nameof(IMAGE_FILE_HEADER.NumberOfSections), NT_HEADERS.FileHeader.NumberOfSections, architecture, "", size:pos),
                CreateDataGridPEItem<IMAGE_FILE_HEADER, uint>(nameof(IMAGE_FILE_HEADER.TimeDateStamp), NT_HEADERS.FileHeader.TimeDateStamp, architecture, "", size:pos),
            };
        }

#pragma warning disable IDE0017
        private DataGridPEItem CreateDataGridPEItem<T, Field>(string memberName, Field member, Architecture architecture, string comment, string? memberNameFixed = null, uint size = 0) where Field : IFormattable
        {
            TypeCode typeCode = Type.GetTypeCode(typeof(Field));
            string fmtToOffset = Module.GetStringFmtByOffsetMode(architecture);
            string fmtToValue = Module.GetStringFmt(typeCode);
            // Format address by architecture and size of type (:
            return new DataGridPEItem()
            {
                Member = memberNameFixed ?? memberName,
                Offset = (size + Marshal.OffsetOf<T>(memberName)).ToString(fmtToOffset),
                Size = typeCode.ToString(),
                Value = member!.ToString(fmtToValue, default),
                Comment = comment,
            };
        }
#pragma warning restore IDE0017

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
                bool result = await WindowService.ShowMessageAsync("Changing the addressing mode for offsets (x86-x64) requires reanalysis of the selected module.");
                if (result)
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