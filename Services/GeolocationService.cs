using AutoMapper;
using DataService.Repository.Interfaces;
using Entities.DbSet;
using Entities.Dtos.Responses;
using IpStack.Services;
using System.Net;

namespace Services;

public class GeolocationService : IGeolocationService
{
    private readonly IIpStackService _ipStackService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GeolocationService(
        IIpStackService ipStackService,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _ipStackService = ipStackService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> DeleteGeolocationData(string ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            return null;
        }

        var ip = IPAddress.TryParse(ipAddress, out IPAddress? result) ? result.ToString() : null;
        if (string.IsNullOrEmpty(ip))
        {
            return null;
        }

        var deletedIp = await _unitOfWork.Geolocations.Delete(ip);
        await _unitOfWork.CompleteAsync();
        return deletedIp;
    }

    public async Task<GeolocationDataResposne?> AddGeolocationData(GeolocationData? data)
    {
        if (data == null || data?.Ip == null)
        {
            return null;
        }

        var ip = IPAddress.TryParse(data.Ip, out IPAddress? result) ? result.ToString() : null;
        if (string.IsNullOrEmpty(ip))
        {
            return null;
        }

        var response = new GeolocationDataResposne();
        var geolocationDataService = await _unitOfWork.Geolocations.Create(data);
        var unitOfWorkResult = await _unitOfWork.CompleteAsync();
        if (geolocationDataService != null && unitOfWorkResult)
        {
            response.Data = geolocationDataService;
            response.IsDownloaded = true;            
        }
        return response;
    }

    public async Task<GeolocationDataResposne?> GetGeolocationData(string? siteAddress) 
    {
        if (string.IsNullOrEmpty(siteAddress))
        {
            return null;
        }

        string? ipAddress = null;
        if (IPAddress.TryParse(siteAddress, out IPAddress? result))
        {
            ipAddress = result.ToString();
        }
        else 
        {
            ipAddress = UrlToIpAddress(siteAddress);
            if (string.IsNullOrEmpty(ipAddress)) 
            {
                return null;
            }
        }

        var response = new GeolocationDataResposne();
        var geolocationDataService = await _unitOfWork.Geolocations.GetByIp(ipAddress);
        if (geolocationDataService != null)
        {
            response.IsDownloaded = true;
            response.Data = geolocationDataService;
            return response;
        }

        var geolocationDataIpStack = _mapper.Map<GeolocationData>(await _ipStackService.GetIpAddressDetailsAsync(ipAddress: ipAddress));

        if (geolocationDataIpStack != null)
        {
            response.IsDownloaded = false;
            response.Data = geolocationDataIpStack;
        }    
        return response;
    }   

    private static string? UrlToIpAddress(string url)
    {
        try
        {
            return Dns.GetHostAddresses(new Uri(url).Host)[0]?.ToString();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Can not extract valid IP from site address");
        }
    }
}
