namespace Entities.DbSet;

public class GeolocationData
{
    public string Ip { get; set; }
    public string ContinentCode { get; set; }
    public string ContinentName { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public string RegionCode { get; set; }
    public string RegionName { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Location Location { get; set; }
}
