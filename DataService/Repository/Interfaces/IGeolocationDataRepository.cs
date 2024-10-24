using Entities.DbSet;

namespace DataService.Repository.Interfaces;

public interface IGeolocationDataRepository : IGenericRepository<GeolocationData>
{
    Task<GeolocationData> GetByIp(string name);
    Task<string> Delete(string name);
}
