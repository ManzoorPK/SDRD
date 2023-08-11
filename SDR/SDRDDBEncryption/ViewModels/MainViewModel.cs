using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SDRDDBEncryption.Contracts.Services;
using SDRDDBEncryption.Contracts.ViewModels;
using SDRDDBEncryption.Core.Contracts.Services;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace SDRDDBEncryption.ViewModels;

public class MainViewModel : ObservableObject, INavigationAware
{
    
    private List<String> tables = new List<String>();

    private bool isRunning;

    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        set
        {
            isRunning = value;
            OnPropertyChanged(nameof(IsRunning));
        }
    }

    private Thread thread;
    private ManualResetEvent resetEvent = new ManualResetEvent(true);

    public List<String> Tables
    {
        get { return tables; }
        set
        {
            tables = value;
            OnPropertyChanged(nameof(Tables));
        }
    }


    private string startPauseButtonText;
    public string StartPauseButtonText
    {
        get
        {
            return startPauseButtonText;
        }
        set
        {
            startPauseButtonText = value;
            OnPropertyChanged(nameof(StartPauseButtonText));

        }
    }

    private List<String> columns = new List<String>();

    public List<String> Columns
    {
        get
        {
            return columns;
        }
        set
        {
            columns = value;
            OnPropertyChanged(nameof(Columns));
        }
    }

    private List<string> SelectionResult { get; set; }

    private List<string> selectedColumns = new List<string>();
    public List<string> SelectedColumns
    {
        get
        {
            if (!String.IsNullOrEmpty(selectedTable) && CommonHelper.EncryptedColumns.ContainsKey(selectedTable))

                return CommonHelper.EncryptedColumns[selectedTable];
            else
                return new List<string>();
        }
        set
        {
            selectedColumns = value;
            OnPropertyChanged(nameof(SelectedColumns));
        }
    }


    //public Dictionary<String, List<String>> EncryptedColumns = new Dictionary<String, List<String>>();

    private string selectedTable;
    public string SelectedTable
    {
        get { return selectedTable; }
        set
        {
            if (value == selectedTable) return;

            selectedTable = value;

            if (CommonHelper.EncryptedColumns.ContainsKey(selectedTable))
            {
                Columns = EncryptionService.GetColumns(value).Except(CommonHelper.EncryptedColumns[selectedTable]).ToList();
            }
            else
            {
                Columns = EncryptionService.GetColumns(value);
            }
            OnPropertyChanged(nameof(SelectedColumns));

        }

    }

    private IEncryptionService EncryptionService { get; set; }

    public MainViewModel(IEncryptionService encryptionService)
    {
        EncryptionService = encryptionService;
        StartPauseButtonText = "Start";

        //Init Commands
        SelectAllCommand = new RelayCommand(SelectAll);
        SelectCommand = new RelayCommand<object>(Select);
        RemoveCommand = new RelayCommand<object>(Remove);
        StartCommand = new RelayCommand(StartEncryption);
        StopCommand = new RelayCommand(Stop);



    }

    public void OnNavigatedFrom() { }
    public void OnNavigatedTo(object parameter)
    {
        Tables = EncryptionService.GetTables();
        CommonHelper.EncryptedColumns = EncryptionService.GetEncryptedTables();

    }
    public ICommand SelectAllCommand { get; }

    public void SelectAll()
    {
        if (!CommonHelper.EncryptedColumns.ContainsKey(SelectedTable))
        {
            CommonHelper.EncryptedColumns.Add(SelectedTable, new List<string>());
        }
        CommonHelper.EncryptedColumns[SelectedTable] = CommonHelper.EncryptedColumns[SelectedTable].Union(Columns).ToList();
        Columns.Clear();
        OnPropertyChanged(nameof(SelectedColumns));
        OnPropertyChanged(nameof(Columns));
    }

    public ICommand SelectCommand { get; }

    public void Select(object selectItemObject)
    {
        if (string.IsNullOrWhiteSpace(SelectedTable))
        {
            return;
        }
        List<string> selectedItems = new List<string>();
        if (selectItemObject != null)
        {

            foreach (var item in (IEnumerable)selectItemObject)
            {
                selectedItems.Add(item.ToString());
            }

        }

        if (!CommonHelper.EncryptedColumns.ContainsKey(SelectedTable))
        {
            CommonHelper.EncryptedColumns.Add(SelectedTable, new List<string>());
        }
        CommonHelper.EncryptedColumns[SelectedTable] = CommonHelper.EncryptedColumns[SelectedTable].Union(selectedItems).ToList();

        Columns = Columns.Except(selectedItems).ToList();

        OnPropertyChanged(nameof(SelectedColumns));
        OnPropertyChanged(nameof(Columns));
    }

    public ICommand RemoveCommand { get; }

    public void Remove(object selectItemObject)
    {
        if (string.IsNullOrWhiteSpace(SelectedTable))
        {
            return;
        }
        List<string> selectedItems = new List<string>();
        if (selectItemObject != null)
        {

            foreach (var item in (IEnumerable)selectItemObject)
            {
                selectedItems.Add(item.ToString());
            }

        }

        if (!CommonHelper.EncryptedColumns.ContainsKey(SelectedTable))
        {
            return;
        }
        CommonHelper.EncryptedColumns[SelectedTable] = CommonHelper.EncryptedColumns[SelectedTable].Except(selectedItems).ToList();

        Columns = Columns.Union(selectedItems).ToList();

        OnPropertyChanged(nameof(SelectedColumns));
        OnPropertyChanged(nameof(Columns));
    }

    public ICommand StartCommand { get; }

    public void StartEncryption()
    {
        if (StartPauseButtonText.Equals("Start", StringComparison.InvariantCultureIgnoreCase))
        {
            StartPauseButtonText = "Pause";

            this.IsRunning = true;

            thread = new Thread(DoTheEncryption);
            thread.Start();

        }
        else
        {

            if (StartPauseButtonText.Equals("Pause", StringComparison.InvariantCultureIgnoreCase))
            {
                this.resetEvent.Reset();
                StartPauseButtonText = "Resume";
            }
            else
            {
                StartPauseButtonText = "Pause";
            }

        }

    }

    public ICommand StopCommand { get; }
    public void Stop()
    {



        this.IsRunning = false;


        this.resetEvent.Set();


        this.thread.Join();


    }


    private void DoTheEncryption()
    {
        foreach (var tablename in CommonHelper.EncryptedColumns.Keys)
        {
            if (!this.IsRunning)
            {
                return;
            }

            var columns = CommonHelper.EncryptedColumns[tablename];


            EncryptionService.EncryptTable(tablename, columns);

            resetEvent.WaitOne();

        }
        //CommonHelper.EncryptedColumns = new Dictionary<string, List<string>>();
        this.IsRunning = false;
        StartPauseButtonText = "Start";
    }

}
