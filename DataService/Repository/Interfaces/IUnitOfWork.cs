namespace DataService.Repository.Interfaces;

public interface IUnitOfWork
{
    IGeolocationDataRepository Geolocations { get; }

    Task<bool> CompleteAsync();
}
