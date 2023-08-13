using Avalonia;
using Avalonia.Controls;

namespace Game_Hacking_Engine.Controls;

public partial class CustomDataGrid : UserControl
{
    public static readonly StyledProperty<object> ItemsSourceProperty =
        AvaloniaProperty.Register<CustomDataGrid, object>(nameof(ItemsSource));

    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public CustomDataGrid()
    {
        InitializeComponent();
        // Bind the property!
        DataGrid.Bind(DataGrid.ItemsSourceProperty, this.GetObservable(ItemsSourceProperty));
    }
}