using Avalonia.Controls;
using Game_Hacking_Engine.Controls;
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
            bool isMaximized = WindowService.IsMaximized(this);
            Padding = isMaximized ? new(Constants.W_PADDING) : new(0);
            FixDataGridScrollViewer(DgDosHeader, isMaximized);
            FixDataGridScrollViewer(DgNtHeaders, isMaximized);
            FixDataGridScrollViewer(DgFileHeaders, isMaximized);
        }

        private void FixDataGridScrollViewer(CustomDataGrid dataGrid, bool isMaximized)
        {
            if (!isMaximized || isMaximized)
            {
                // Enforces recalculation of the DataGrid's height; for some reason, it is not being recalculated when exiting Maximized mode.
                dataGrid.Height = double.MaxValue;
                dataGrid.UpdateLayout();
                dataGrid.Height = double.NaN;
            }
        }
    }
}