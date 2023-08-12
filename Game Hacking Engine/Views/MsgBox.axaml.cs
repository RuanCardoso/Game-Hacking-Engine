using Avalonia.Controls;
using Game_Hacking_Engine.ViewModels;

namespace Game_Hacking_Engine.Views;

public partial class MsgBox : Window
{
    public MsgBox()
    {
        InitializeComponent();
    }

    public void Initialize(string title, string message)
    {
        MsgBoxViewModel viewModel = (MsgBoxViewModel)DataContext!;
        viewModel.Title = title;
        viewModel.Message = message;
    }
}