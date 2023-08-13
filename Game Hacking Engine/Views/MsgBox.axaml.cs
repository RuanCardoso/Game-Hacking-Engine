using Avalonia.Controls;
using Game_Hacking_Engine.ViewModels;

namespace Game_Hacking_Engine.Views;

public partial class MsgBox : Window
{
    private MsgBoxViewModel? viewModel;
    public MsgBox()
    {
        InitializeComponent();
    }

    public void Initialize(string title, string message)
    {
        viewModel = (MsgBoxViewModel)DataContext!;
        viewModel.Title = title;
        viewModel.Message = message;
    }

    public bool GetResult()
    {
        return viewModel!.IsOk;
    }
}