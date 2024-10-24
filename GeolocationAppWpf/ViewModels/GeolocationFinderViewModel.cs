using GeolocationAppWpf.Commands;
using GeolocationAppWpf.Models;
using System.Windows.Input;

namespace GeolocationAppWpf.ViewModels;

public class GeolocationFinderViewModel : ViewModelBase
{
    private string _searchTextBox;
    public string SearchTextBox
    {
        get 
        {
            return _searchTextBox;
        }
        set 
        {
            _searchTextBox = value;
            OnPropertyChanged(nameof(SearchTextBox));
        } 
    }

    private string _dataTextBox;
    public string DataTextBox
    {
        get
        {
            return _dataTextBox;
        }
        set
        {
            _dataTextBox = value;
            OnPropertyChanged(nameof(DataTextBox));
        }
    }

    private string _syncTextBlock;
    public string SyncTextBlock
    {
        get
        {
            return _syncTextBlock;
        }
        set
        {
            _syncTextBlock = value;
            OnPropertyChanged(nameof(SyncTextBlock));
        }
    }

    string _infoTextBlock;
    public string InfoTextBlock
    {
        get
        {
            return _infoTextBlock;
        }
        set
        {
            _infoTextBlock = value;
            OnPropertyChanged(nameof(InfoTextBlock));
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }

    public GeolocationFinderViewModel(Geolocation _geolocation)
    {
        AddCommand = new AddGeolocationCommand(this, _geolocation);
        SearchCommand = new SearchGeolocationCommand(this, _geolocation);
        DeleteCommand = new RemoveGeolocationDataCommand(this, _geolocation);
    }
}
