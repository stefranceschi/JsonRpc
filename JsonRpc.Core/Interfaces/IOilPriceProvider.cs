using JsonRpc.Core.DTOs;

namespace JsonRpc.Core.Interfaces
{
    public interface IOilPriceProvider
    {
         Task<List<OilPriceDto>> GetOilPrices(DateTime startDate, DateTime endDate);
    }
}