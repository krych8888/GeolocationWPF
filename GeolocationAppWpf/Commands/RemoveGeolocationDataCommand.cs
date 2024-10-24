using Entities.DbSet;
using GeolocationAppWpf.Models;
using GeolocationAppWpf.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GeolocationAppWpf.Commands;

public class RemoveGeolocationDataCommand : AsyncComandBase
{
    private readonly GeolocationFinderViewModel _model;
    private readonly Geolocation _geolocation;

    public RemoveGeolocationDataCommand(GeolocationFinderViewModel model, Geolocation geolocation)
    {
        _model = model;
        _geolocation = geolocation;

        _model.PropertyChanged += OnViewModelPropertyChanged; 
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        if (!_geolocation.IsDownloaded) 
        {
            return;
        }

        var newGeolocation = JsonConvert.DeserializeObject<GeolocationData>(_model.DataTextBox);

        if (newGeolocation != null)
        {
            var deletedIp = await _geolocation.Delete(newGeolocation);           
            if (deletedIp != null)
            {
                _geolocation.IsDownloaded = false;
                _model.SyncTextBlock = "Not synchronized";
            }
            else 
            {
                _model.DataTextBox = "Something went wrong during removing item from DB";
            }
        }
        else
        {
            _model.DataTextBox = "GeolocationData model not valid";
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_model.DataTextBox)
            && _geolocation.IsDownloaded
            && base.CanExecute(parameter);
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GeolocationFinderViewModel.SyncTextBlock)) 
        {
            OnCanExecutedChanged();
        }
    }
}
