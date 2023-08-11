using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using SDRDBE.Contracts.ViewModels;
using SDRDBE.Models;
using SDRDDBEncryption.Core.Contracts.Services;
using SDRDDBEncryption.Core.Models;
using SDRDDBEncryption.Core.Services;

namespace SDRDBE.ViewModels;

public class SearchPageViewModel : ObservableObject, INavigationAware
{
    private readonly IDatabaseService DatabaseService;
    
    public SearchResults Selected
    { get; set; }

    private readonly AppConfig Config;


    public ObservableCollection<SearchResults> SearchResultItems { get; private set; } = new ObservableCollection<SearchResults>();

    public SearchPageViewModel(IDatabaseService dataService, IOptions<AppConfig> appConfig)
    {
        DatabaseService = dataService;
        DatabaseService.SetConnectionString(appConfig.Value.ConnectionString);

    }

    public async void OnNavigatedTo(object parameter)
    {
        //SearchResultItems.Clear();

        //var data = await databaseService.GetSearchPageDataAsync();

        //foreach (var item in data)
        //{
        //    SearchResultItems.Add(item);
        //}

        //Selected = SearchResultItems.First();
    }

    public void OnNavigatedFrom()
    {
    }

    public void Search(string e)
    {
        SearchResultItems =new ObservableCollection<SearchResults>( DatabaseService.GetSearchResults(e));


    }

}
