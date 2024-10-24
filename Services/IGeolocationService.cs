using Entities.DbSet;
using Entities.Dtos.Responses;

namespace Services;

public interface IGeolocationService
{
    public Task<GeolocationDataResposne?> GetGeolocationData(string? url);
    public Task<GeolocationDataResposne?> AddGeolocationData(GeolocationData? data);
    public Task<string?> DeleteGeolocationData(string ipAddress);
}
