using GeolocationAppWpf.Models;

namespace GeolocationAppWpf.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel { get; }

    public MainViewModel(Geolocation geolocation)
    {
        CurrentViewModel = new GeolocationFinderViewModel(geolocation);
    }
}