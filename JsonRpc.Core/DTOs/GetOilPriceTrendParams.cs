namespace JsonRpc.Core.DTOs
{
    public class GetOilPriceTrendParams
    {
        public DateTime? StartDateISO8601 { get; set; }
        public DateTime? EndDateISO8601 { get; set; }
    }
}