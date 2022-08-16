using System.Text.Json;
using AutoMapper;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Enums;
using JsonRpc.Core.Errors;
using JsonRpc.Core.Interfaces;
using JsonRpc.Core.Model;
using Microsoft.Extensions.Configuration;

namespace JsonRpc.Core.DataProviders
{
    public class OilPriceProvider : IOilPriceProvider
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public OilPriceProvider(HttpClient client, IMapper mapper, IConfiguration config)
        {
            _client = client;
            _mapper = mapper;
            _config = config;
        }

        public async Task<List<OilPriceDto>> GetOilPrices(DateTime startDate, DateTime endDate)
        {
            var apiUrl = _config[ConfigTypes.OilPricesApiUrl];
            if (string.IsNullOrEmpty(apiUrl))
                throw new RestException(System.Net.HttpStatusCode.InternalServerError, new { OilPricesApiUrl = "Missing configuration" });

            var data = await GetDataFromApi(apiUrl);
            var prices = data.Where(x => x.Date >= startDate && x.Date <= endDate).OrderBy(x => x.Date).ToList();

            var retVal = _mapper.Map<List<OilPrice>, List<OilPriceDto>>(prices);
            return retVal;
        }

        private async Task<List<OilPrice>> GetDataFromApi(string apiUrl)
        {
            var retVal = new List<OilPrice>();

            try
            {
                var response = await _client.GetStringAsync(apiUrl);
                var jsonOpt = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                retVal = JsonSerializer.Deserialize<List<OilPrice>>(response, jsonOpt);
            }
            catch (Exception)
            {
                throw new RestException(System.Net.HttpStatusCode.InternalServerError, new { Data = "Error deserealizing data" });
            }

            return retVal;
        }
    }
}