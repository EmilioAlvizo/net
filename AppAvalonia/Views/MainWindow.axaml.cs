using Avalonia.Controls;

namespace AppAvalonia.Views;

public partial class MainWindow : Window
{
    public ContentControl? MainContentControl => this.FindControl<ContentControl>("MainContent");
    public MainWindow()
    {
        InitializeComponent();
    }
}
