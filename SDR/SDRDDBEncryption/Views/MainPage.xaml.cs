using System.Windows.Controls;

using SDRDDBEncryption.ViewModels;

namespace SDRDDBEncryption.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void ColumnSelectAllButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
