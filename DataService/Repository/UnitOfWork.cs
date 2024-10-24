using DataService.Data;
using DataService.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace DataService.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;
    
    public IGeolocationDataRepository Geolocations { get; }

    public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        var logger = loggerFactory.CreateLogger("logs");

        Geolocations = new GeolocationDataRepository(_context, logger);
    }

    public async Task<bool> CompleteAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public void Dispose() 
    {
        _context.Dispose();
    }
}
