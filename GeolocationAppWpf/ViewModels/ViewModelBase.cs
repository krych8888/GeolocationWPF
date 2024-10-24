using System.ComponentModel;

namespace GeolocationAppWpf.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string porpertyName) 
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(porpertyName));
    }
}
