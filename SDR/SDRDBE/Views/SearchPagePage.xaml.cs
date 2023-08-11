using System.Windows.Controls;
using System.Windows.Input;
using SDRDBE.ViewModels;
using Windows.UI.Core;

namespace SDRDBE.Views;

public partial class SearchPagePage : Page
{

    SearchPageViewModel ViewModel;
    public SearchPagePage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        // Static constructor

        // Bind for Ctrl + F => Activate search.
        CommandManager.RegisterClassCommandBinding(
                 typeof(SearchPagePage),
                 new CommandBinding(
                     new RoutedCommand(
                         "Find", typeof(SearchPagePage),
                         new InputGestureCollection(){
                            new KeyGesture(Key.F, ModifierKeys.Control)
                         }
                     ),
                     (a, b) => (a as SearchPagePage)?.SearchBox.ActivateSearch()
                 )
             );
    }

    private void SearchBox_SearchRequested(object sender, string e)
    {
        ViewModel.Search(e);
    }
}

