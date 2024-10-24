using Entities.DbSet;

namespace Entities.Dtos.Responses;

public class GeolocationDataResposne
{
    public GeolocationData Data { get; set; }
    public bool IsDownloaded { get; set; } = false;
}
