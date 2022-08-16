using AutoMapper;
using JsonRpc.Core.DTOs;

namespace JsonRpc.Core.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GenericJsonRpcRequest, GetOilPriceTrendRequest>();
        }
    }
}