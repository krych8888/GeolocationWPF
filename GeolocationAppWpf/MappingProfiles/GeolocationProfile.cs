using AutoMapper;
using Entities.DbSet;
using IpStack.Models;

namespace EventBookingApi.MappingProfiles;

public class GeolocationProfile : Profile
{
    public GeolocationProfile()
    {
        CreateMap<IpAddressDetails, GeolocationData>();

        CreateMap<IpStack.Models.Location, Entities.DbSet.Location>();        
    }
}
