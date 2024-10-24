using DataService.Data;
using DataService.Repository.Interfaces;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataService.Repository;

public class GeolocationDataRepository : GenericRepostory<GeolocationData>, IGeolocationDataRepository
{
    public GeolocationDataRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<GeolocationData> GetByIp(string ip)
    {
        try
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Ip == ip);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetByIp function error", typeof(GeolocationDataRepository));
            throw;
        }
    }
    
    public async Task<string> Delete(string ip)
    {
        try
        {
            var geolocationToDelete = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Ip == ip);
            if (geolocationToDelete == null)
            {
                return null;
            }
            _context.ChangeTracker.Clear();
            _dbSet.Remove(geolocationToDelete);
            return ip;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Delete function error", typeof(GeolocationDataRepository));
            throw;
        }
    }

    public override async Task<GeolocationData?> Create(GeolocationData newGeolocation)
    {
        try
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Ip == newGeolocation.Ip);
            if (entity != null)
            {
                throw new ArgumentException($"Geolocation for {newGeolocation.Ip} already exists");
            }

            return await base.Create(newGeolocation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Create function error", typeof(GeolocationDataRepository));
            throw;
        }
    }
}
