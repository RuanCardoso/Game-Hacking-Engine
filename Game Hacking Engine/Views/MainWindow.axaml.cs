using Avalonia.Controls;
using Game_Hacking_Engine.Services;

namespace Game_Hacking_Engine.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (WindowManager.AddWindow(this, Windows.Main))
            {
                InitializeComponent();
                // Prevent maximize bug(bigger than screen)
                Resized += (_, _) => OnMaximized();
            }
        }

        private void OnMaximized()
        {
            Padding = WindowService.IsMaximized(this) ? new(Constants.W_PADDING) : new(0);
        }
    }
}