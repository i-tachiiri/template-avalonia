using Avalonia.Controls;

namespace Presentation.Desktop;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }
}
