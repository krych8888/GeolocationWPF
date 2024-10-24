using AutoMapper;
using Entities.DbSet;
using IpStack.Models;

namespace EventBookingApi.MappingProfiles;

public class DamoinToResponse : Profile
{
    public DamoinToResponse()
    {
        CreateMap<IpAddressDetails, GeolocationData>();

        CreateMap<IpStack.Models.Location, Entities.DbSet.Location>();

        //CreateMap<GeolocationData, GeolocationDataResposne>()
        //    .ForMember(x => x.Url, opt => opt.Ignore())
        //    .ForMember(x => x.IsDownloaded, opt => opt.Ignore());        
    }
}
