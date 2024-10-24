using Entities.DbSet;
using GeolocationAppWpf.Models;
using GeolocationAppWpf.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GeolocationAppWpf.Commands;

public class AddGeolocationCommand : AsyncComandBase
{
    private readonly GeolocationFinderViewModel _model;
    private readonly Geolocation _geolocation;

    public AddGeolocationCommand(GeolocationFinderViewModel model, Geolocation geolocation)
    {
        _model = model;
        _geolocation = geolocation;

        _model.PropertyChanged += OnViewModelPropertyChanged; 
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        var newGeolocation = JsonConvert.DeserializeObject<GeolocationData>(_model.DataTextBox);
        if (newGeolocation != null)
        {
            var response = await _geolocation.AddGeolocationData(newGeolocation);
            if (response != null)
            {
                _geolocation.Data = _geolocation.FormatData(response.Data);
                _geolocation.IsDownloaded = true;
                _model.SyncTextBlock = "Synchronized";
            }
            else 
            {
                _geolocation.Data = "Something went wrong during adding to DB";
                _geolocation.IsDownloaded = false;
                _model.SyncTextBlock = "Not synchronized";
            }            
        }
        else
        {
            _geolocation.Data = "GeolocationData model not valid";
            _geolocation.IsDownloaded = false;
            _model.SyncTextBlock = "Not synchronized";
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_model.DataTextBox)
            && !_geolocation.IsDownloaded
            && base.CanExecute(parameter);
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GeolocationFinderViewModel.DataTextBox) ||
            e.PropertyName == nameof(GeolocationFinderViewModel.SyncTextBlock)) 
        {
            OnCanExecutedChanged();
        }
    }
}
