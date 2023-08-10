using Avalonia.Controls;
using Game_Hacking_Engine.ViewModels;

namespace Game_Hacking_Engine.Views;

public partial class FileDialogWindow : Window
{
    public FileDialogWindow()
    {
        InitializeComponent();
    }

    public void Initialize() // Hack..... MVVM sucks!! BINDING
    {
        var viewModel = (FileDialogWindowViewModel)DataContext!;
        viewModel.GridTree = GridTree;
        viewModel.LeftTreeView = LeftTreeView;
        viewModel.RightTreeView = RightTreeView;
        viewModel.Initialize();
        // Initialize Events
        PathControl.TextChanged += viewModel.OnTextChanged;
        Resized += viewModel.OnResized;
    }
}