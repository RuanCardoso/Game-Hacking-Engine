using System.IO;
using System.Runtime.InteropServices;

namespace Game_Hacking_Engine.Services
{
    public enum IMAGE_DIRECTORY_ENTRY : int
    {
        EXPORT = 0,
        IMPORT = 1,
        RESOURCE = 2,
        EXCEPTION = 3,
        SECURITY = 4,
        BASERELOC = 5,
        DEBUG = 6,
        ARCHITECTURE = 7,
        GLOBALPTR = 8,
        TLS = 9,
        LOAD_CONFIG = 10,
        BOUND_IMPORT = 11,
        IAT = 12,
        DELAY_IMPORT = 13,
        COM_DESCRIPTOR = 14
    }

    enum Architecture
    {
        x86,
        x64,
        Unk
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DOS_HEADER
    {
        public ushort e_magic;       // Magic number ("MZ" for DOS executable)
        public ushort e_cblp;        // Bytes on last page of file
        public ushort e_cp;          // Pages in file
        public ushort e_crlc;        // Relocations
        public ushort e_cparhdr;     // Size of header in paragraphs
        public ushort e_minalloc;    // Minimum extra paragraphs needed
        public ushort e_maxalloc;    // Maximum extra paragraphs needed
        public ushort e_ss;          // Initial (relative) SS value
        public ushort e_sp;          // Initial SP value
        public ushort e_csum;        // Checksum
        public ushort e_ip;          // Initial IP value
        public ushort e_cs;          // Initial (relative) CS value
        public ushort e_lfarlc;      // File address of relocation table
        public ushort e_ovno;        // Overlay number
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] e_res;       // Reserved words
        public ushort e_oemid;       // OEM identifier (for e_oeminfo)
        public ushort e_oeminfo;     // OEM information; e_oemid specific
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ushort[] e_res2;      // Reserved words
        public uint e_lfanew;         // File address of new exe header(PE -> NT_HEADERS)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DATA_DIRECTORY
    {
        public uint VirtualAddress;
        public uint Size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_FILE_HEADER
    {
        public ushort Machine;              // Arquitetura alvo do executável
        public ushort NumberOfSections;     // Número de seções no arquivo
        public uint TimeDateStamp;          // Data e hora da criação do arquivo
        public uint PointerToSymbolTable;   // Ponteiro para a tabela de símbolos (não utilizado na maioria dos executáveis)
        public uint NumberOfSymbols;        // Número de símbolos na tabela de símbolos (não utilizado na maioria dos executáveis)
        public ushort SizeOfOptionalHeader; // Tamanho do cabeçalho opcional (32 bits)
        public ushort Characteristics;      // Características do arquivo
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_OPTIONAL_HEADER32
    {
        public ushort Magic;                 // Tipo de formato (PE32 ou PE32+)
        public byte MajorLinkerVersion;      // Versão principal do linker
        public byte MinorLinkerVersion;      // Versão secundária do linker
        public uint SizeOfCode;              // Tamanho da seção de código
        public uint SizeOfInitializedData;   // Tamanho da seção de dados inicializados
        public uint SizeOfUninitializedData; // Tamanho da seção de dados não inicializados
        public uint AddressOfEntryPoint;     // Endereço da entrada do ponto de entrada
        public uint BaseOfCode;              // Endereço base da seção de código
        public uint BaseOfData;              // Endereço base da seção de dados
        public uint ImageBase;               // Endereço base da imagem quando carregada em memória
        public uint SectionAlignment;        // Alinhamento das seções em memória
        public uint FileAlignment;           // Alinhamento das seções no arquivo
        public ushort MajorOperatingSystemVersion; // Versão principal do sistema operacional alvo
        public ushort MinorOperatingSystemVersion; // Versão secundária do sistema operacional alvo
        public ushort MajorImageVersion;     // Versão principal da imagem
        public ushort MinorImageVersion;     // Versão secundária da imagem
        public ushort MajorSubsystemVersion; // Versão principal do subsistema
        public ushort MinorSubsystemVersion; // Versão secundária do subsistema
        public uint Win32VersionValue;       // Reservado, não utilizado
        public uint SizeOfImage;             // Tamanho total da imagem carregada em memória
        public uint SizeOfHeaders;           // Tamanho dos cabeçalhos PE
        public uint CheckSum;                // Soma de verificação do arquivo
        public ushort Subsystem;             // Subsistema necessário para a execução
        public ushort DllCharacteristics;    // Características do arquivo DLL
        public uint SizeOfStackReserve;      // Tamanho reservado da pilha
        public uint SizeOfStackCommit;       // Tamanho da pilha inicialmente comprometido
        public uint SizeOfHeapReserve;       // Tamanho reservado do heap
        public uint SizeOfHeapCommit;        // Tamanho do heap inicialmente comprometido
        public uint LoaderFlags;             // Sinalizadores do carregador
        public uint NumberOfRvaAndSizes;     // Número de diretórios de dados na estrutura IMAGE_DATA_DIRECTORY
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public IMAGE_DATA_DIRECTORY[] DataDirectory;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_NT_HEADERS
    {
        public uint Signature;               // Assinatura do cabeçalho PE ("PE\0\0")
        public IMAGE_FILE_HEADER FileHeader; // Cabeçalho do arquivo
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader; // Cabeçalho opcional (32 bits)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PE_HEADER
    {
        public IMAGE_DOS_HEADER IMAGE_DOS_HEADER;
        public IMAGE_NT_HEADERS IMAGE_NT_HEADERS;

        public PE_HEADER(IMAGE_DOS_HEADER iMAGE_DOS_HEADER, IMAGE_NT_HEADERS iMAGE_NT_HEADERS)
        {
            IMAGE_DOS_HEADER = iMAGE_DOS_HEADER;
            IMAGE_NT_HEADERS = iMAGE_NT_HEADERS;
        }
    }

    internal class PE
    {
        // yeah i know i could use marshall here but i don't want to.
        // Marshal.PtrToStructure.... i know
        public static PE_HEADER GetPEHeader(string path)
        {
            try
            {
                using (FileStream flStream = new(path, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader bin = new(flStream))
                    {
                        IMAGE_DOS_HEADER IMAGE_DOS_HEADER;
                        IMAGE_NT_HEADERS IMAGE_NT_HEADERS;

                        #region DOS_HEADER
                        IMAGE_DOS_HEADER.e_magic = bin.ReadUInt16();      // Magic number ("MZ" for DOS executable)
                        IMAGE_DOS_HEADER.e_cblp = bin.ReadUInt16();       // Bytes on last page of file
                        IMAGE_DOS_HEADER.e_cp = bin.ReadUInt16();         // Pages in file
                        IMAGE_DOS_HEADER.e_crlc = bin.ReadUInt16();       // Relocations
                        IMAGE_DOS_HEADER.e_cparhdr = bin.ReadUInt16();    // Size of header in paragraphs
                        IMAGE_DOS_HEADER.e_minalloc = bin.ReadUInt16();   // Minimum extra paragraphs needed
                        IMAGE_DOS_HEADER.e_maxalloc = bin.ReadUInt16();   // Maximum extra paragraphs needed
                        IMAGE_DOS_HEADER.e_ss = bin.ReadUInt16();         // Initial (relative) SS value
                        IMAGE_DOS_HEADER.e_sp = bin.ReadUInt16();         // Initial SP value
                        IMAGE_DOS_HEADER.e_csum = bin.ReadUInt16();       // Checksum
                        IMAGE_DOS_HEADER.e_ip = bin.ReadUInt16();         // Initial IP value
                        IMAGE_DOS_HEADER.e_cs = bin.ReadUInt16();         // Initial (relative) CS value
                        IMAGE_DOS_HEADER.e_lfarlc = bin.ReadUInt16();     // File address of relocation table
                        IMAGE_DOS_HEADER.e_ovno = bin.ReadUInt16();       // Overlay number
                        IMAGE_DOS_HEADER.e_res = new ushort[4];
                        for (int i = 0; i < 4; i++)
                        {
                            IMAGE_DOS_HEADER.e_res[i] = bin.ReadUInt16(); // Reserved words
                        }
                        IMAGE_DOS_HEADER.e_oemid = bin.ReadUInt16();      // OEM identifier (for e_oeminfo)
                        IMAGE_DOS_HEADER.e_oeminfo = bin.ReadUInt16();    // OEM information; e_oemid specific
                        IMAGE_DOS_HEADER.e_res2 = new ushort[10];
                        for (int i = 0; i < 10; i++)
                        {
                            IMAGE_DOS_HEADER.e_res2[i] = bin.ReadUInt16(); // Reserved words
                        }
                        IMAGE_DOS_HEADER.e_lfanew = bin.ReadUInt32();      // File address of new exe header(NT_Headers)
                        #endregion

                        #region NT_HEADERS
                        flStream.Seek(IMAGE_DOS_HEADER.e_lfanew, SeekOrigin.Begin);
                        IMAGE_NT_HEADERS.Signature = bin.ReadUInt32();
                        IMAGE_NT_HEADERS.FileHeader.Machine = bin.ReadUInt16();
                        IMAGE_NT_HEADERS.FileHeader.NumberOfSections = bin.ReadUInt16();
                        IMAGE_NT_HEADERS.FileHeader.TimeDateStamp = bin.ReadUInt32();
                        IMAGE_NT_HEADERS.FileHeader.PointerToSymbolTable = bin.ReadUInt32();
                        IMAGE_NT_HEADERS.FileHeader.NumberOfSymbols = bin.ReadUInt32();
                        IMAGE_NT_HEADERS.FileHeader.SizeOfOptionalHeader = bin.ReadUInt16();
                        IMAGE_NT_HEADERS.FileHeader.Characteristics = bin.ReadUInt16();
                        // OPTIONAL HEADER
                        IMAGE_NT_HEADERS.OptionalHeader.Magic = bin.ReadUInt16();           // Tipo de formato (PE32 ou PE32+)
                        IMAGE_NT_HEADERS.OptionalHeader.MajorLinkerVersion = bin.ReadByte();   // Versão principal do linker
                        IMAGE_NT_HEADERS.OptionalHeader.MinorLinkerVersion = bin.ReadByte();      // Versão secundária do linker
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfCode = bin.ReadUInt32();             // Tamanho da seção de código
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfInitializedData = bin.ReadUInt32();   // Tamanho da seção de dados inicializados
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfUninitializedData = bin.ReadUInt32(); // Tamanho da seção de dados não inicializados
                        IMAGE_NT_HEADERS.OptionalHeader.AddressOfEntryPoint = bin.ReadUInt32();    // Endereço da entrada do ponto de entrada
                        IMAGE_NT_HEADERS.OptionalHeader.BaseOfCode = bin.ReadUInt32();             // Endereço base da seção de código
                        IMAGE_NT_HEADERS.OptionalHeader.BaseOfData = bin.ReadUInt32();            // Endereço base da seção de dados
                        IMAGE_NT_HEADERS.OptionalHeader.ImageBase = bin.ReadUInt32();            // Endereço base da imagem quando carregada em memória
                        IMAGE_NT_HEADERS.OptionalHeader.SectionAlignment = bin.ReadUInt32();         // Alinhamento das seções em memória
                        IMAGE_NT_HEADERS.OptionalHeader.FileAlignment = bin.ReadUInt32();            // Alinhamento das seções no arquivo
                        IMAGE_NT_HEADERS.OptionalHeader.MajorOperatingSystemVersion = bin.ReadUInt16(); // Versão principal do sistema operacional alvo
                        IMAGE_NT_HEADERS.OptionalHeader.MinorOperatingSystemVersion = bin.ReadUInt16(); // Versão secundária do sistema operacional alvo
                        IMAGE_NT_HEADERS.OptionalHeader.MajorImageVersion = bin.ReadUInt16();     // Versão principal da imagem
                        IMAGE_NT_HEADERS.OptionalHeader.MinorImageVersion = bin.ReadUInt16();     // Versão secundária da imagem
                        IMAGE_NT_HEADERS.OptionalHeader.MajorSubsystemVersion = bin.ReadUInt16(); // Versão principal do subsistema
                        IMAGE_NT_HEADERS.OptionalHeader.MinorSubsystemVersion = bin.ReadUInt16(); // Versão secundária do subsistema
                        IMAGE_NT_HEADERS.OptionalHeader.Win32VersionValue = bin.ReadUInt32();       // Reservado, não utilizado
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfImage = bin.ReadUInt32();             // Tamanho total da imagem carregada em memória
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfHeaders = bin.ReadUInt32();           // Tamanho dos cabeçalhos PE
                        IMAGE_NT_HEADERS.OptionalHeader.CheckSum = bin.ReadUInt32();                // Soma de verificação do arquivo
                        IMAGE_NT_HEADERS.OptionalHeader.Subsystem = bin.ReadUInt16();             // Subsistema necessário para a execução
                        IMAGE_NT_HEADERS.OptionalHeader.DllCharacteristics = bin.ReadUInt16();    // Características do arquivo DLL
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfStackReserve = bin.ReadUInt32();      // Tamanho reservado da pilha
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfStackCommit = bin.ReadUInt32();       // Tamanho da pilha inicialmente comprometido
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfHeapReserve = bin.ReadUInt32();       // Tamanho reservado do heap
                        IMAGE_NT_HEADERS.OptionalHeader.SizeOfHeapCommit = bin.ReadUInt32();        // Tamanho do heap inicialmente comprometido
                        IMAGE_NT_HEADERS.OptionalHeader.LoaderFlags = bin.ReadUInt32();             // Sinalizadores do carregador
                        IMAGE_NT_HEADERS.OptionalHeader.NumberOfRvaAndSizes = bin.ReadUInt32();     // Número de diretórios de dados na estrutura IMAGE_DATA_DIRECTORY
                        IMAGE_NT_HEADERS.OptionalHeader.DataDirectory = new IMAGE_DATA_DIRECTORY[16];
                        for (int i = 0; i < 16; i++)
                        {
                            IMAGE_NT_HEADERS.OptionalHeader.DataDirectory[i].VirtualAddress = bin.ReadUInt32();
                            IMAGE_NT_HEADERS.OptionalHeader.DataDirectory[i].Size = bin.ReadUInt32();
                        }
                        #endregion

                        return new PE_HEADER(IMAGE_DOS_HEADER, IMAGE_NT_HEADERS);
                    }
                }
            }
            catch
            {
                WindowService.ShowMessage("The process is already in use. Please wait for a few moments and try again. If the issue persists, please contact support.");
                return default;
            }
        }

        public static Architecture GetArchitectureOfPEHeader(ushort optionalHeaderMagic)
        {
            return optionalHeaderMagic switch
            {
                0x10B => Architecture.x86, // PE32
                0x20B => Architecture.x64, // PE32+
                _ => Architecture.Unk,
            };
        }
    }
}
