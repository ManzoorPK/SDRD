using System.Windows.Controls;

using SDRDBE.ViewModels;

namespace SDRDBE.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
