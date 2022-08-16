using AutoMapper;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Model;

namespace JsonRpc.Core.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GenericJsonRpcRequest, GetOilPriceTrendRequest>();
            
            CreateMap<OilPrice, OilPriceDto>()
                .ForMember(dest => dest.DateISO8601, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
        }
    }
}