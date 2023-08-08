using Avalonia.Controls;
using Game_Hacking_Engine.ViewModels;

namespace Game_Hacking_Engine.Views;

public partial class FileDialogWindow : Window
{
    public FileDialogWindow()
    {
        InitializeComponent();
    }

    public void Initialize() // Hack.....
    {
        var viewModel = (FileDialogWindowViewModel)DataContext!;
        viewModel.GridTree = GridTree;
        viewModel.LeftTreeView = LeftTreeView;
        viewModel.RightTreeView = RightTreeView;
        PathControl.TextChanged += viewModel.OnTextChanged;
        viewModel.Initialize();
        // Others events
        Resized += viewModel.OnResized;
    }
}