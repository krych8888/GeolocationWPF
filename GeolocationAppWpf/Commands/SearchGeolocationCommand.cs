using GeolocationAppWpf.Models;
using GeolocationAppWpf.ViewModels;
using System.ComponentModel;

namespace GeolocationAppWpf.Commands;

public class SearchGeolocationCommand : AsyncComandBase
{
    private readonly GeolocationFinderViewModel _model;
    private readonly Geolocation _geolocation;

    public SearchGeolocationCommand(GeolocationFinderViewModel model, Geolocation geolocation)
    {
        _model = model;
        _geolocation = geolocation;

        _model.PropertyChanged += OnViewModelPropertyChanged; 
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        try 
        {
            _model.ErrorMessage = string.Empty;
            var response = await _geolocation.SearchGeolocationData(_model.SearchTextBox);
            if (response?.Data != null)
            {
                if (response.Data.Ip == null)
                {
                    _model.DataTextBox = string.Empty;
                    _model.ErrorMessage = "IpStack API not avilable, please upgread your subscription plan or change API key";
                }
                else 
                {
                    _model.InfoTextBlock = $"Geolocation data for: {response.Data.Ip}";
                    _model.DataTextBox = _geolocation.FormatData(response.Data);
                    _geolocation.IsDownloaded = response.IsDownloaded;
                    _model.SyncTextBlock = response.IsDownloaded ? "Synchronized" : "Not synchronized";
                }
            }
            else
            {
                _model.ErrorMessage = "Not found";
                _geolocation.IsDownloaded = false;
                _model.SyncTextBlock = "Not synchronized";
            }
        }
        catch (Exception ex)
        {
            _model.DataTextBox = string.Empty;
            _model.ErrorMessage = ex.Message;
        }        
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_model.SearchTextBox)
            && base.CanExecute(parameter);
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GeolocationFinderViewModel.SearchTextBox)) 
        {
            OnCanExecutedChanged();
        }
    }   
}
