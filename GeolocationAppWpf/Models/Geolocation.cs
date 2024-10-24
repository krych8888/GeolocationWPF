using Entities.DbSet;
using Entities.Dtos.Responses;
using Services;
using System.Text;

namespace GeolocationAppWpf.Models;

public class Geolocation
{
    public string Data { get; set; }
    public bool IsDownloaded { get; set; }
    private readonly IGeolocationService _geolocationService;


    public Geolocation(IGeolocationService geolocationService)
    {
        _geolocationService = geolocationService;
    }

    public async Task<GeolocationDataResposne?> SearchGeolocationData(string data)
    {
        return await _geolocationService.GetGeolocationData(data);        
    }

    public async Task<GeolocationDataResposne?> AddGeolocationData(GeolocationData data)
    {
        return await _geolocationService.AddGeolocationData(data);
    }

    public async Task<string?> Delete(GeolocationData data)
    {
        if (!string.IsNullOrEmpty(data?.Ip)) 
        {
            return await _geolocationService.DeleteGeolocationData(data.Ip);
        }
        return null;
    }

    public string FormatData(GeolocationData data)
    {
        var stringData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        return System.Text.Json.Nodes.JsonNode.Parse(Encoding.UTF8.GetBytes(stringData)).ToString(); ;
    }
}
