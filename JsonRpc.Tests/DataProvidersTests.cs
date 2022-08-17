using AutoMapper;
using JsonRpc.Core.Commands;
using JsonRpc.Core.DataProviders;
using JsonRpc.Core.Errors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace JsonRpc.Tests
{
    [TestClass]
    public class DataProvidersTests
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DataProvidersTests()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration configuration = configBuilder.Build();

            var services = new ServiceCollection();
            services.AddScoped<IConfiguration>(_ => configuration);
            services.AddAutoMapper(typeof(GetOilPriceTrend.Handler));

            var serviceProvider = services.BuildServiceProvider();
            _config = serviceProvider.GetService<IConfiguration>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        [TestMethod]
        public async Task OilPriceProvider_GetOilPrices_ReturnSomeData()
        {
            var provider = new OilPriceProvider(new HttpClient(), _mapper, _config);
            var startDate = new DateTime(2022, 01, 01);
            var endDate = DateTime.Now;
            var prices = await provider.GetOilPrices(startDate, endDate);

            Assert.IsNotNull(prices);
            Assert.IsTrue(prices.Count > 0);
        }

        [TestMethod]
        public async Task OilPriceProvider_GetOilPrices_DateAreBetweenInput()
        {
            var provider = new OilPriceProvider(new HttpClient(), _mapper, _config);
            var startDate = new DateTime(2022, 07, 01);
            var endDate = new DateTime(2022, 07, 30);
            var prices = await provider.GetOilPrices(startDate, endDate);

            foreach (var price in prices)
            {
                DateTime currentDate;
                var parsingResult = DateTime.TryParse(price.DateISO8601, out currentDate);
                Assert.IsTrue(parsingResult);

                var isBetweenInput = currentDate >= startDate && currentDate <= endDate;
                Assert.IsTrue(isBetweenInput);
            }
        }

        [TestMethod]
        public async Task OilPriceProvider_GetOilPrices_ValidPrices()
        {
            var provider = new OilPriceProvider(new HttpClient(), _mapper, _config);
            var startDate = new DateTime(2022, 07, 01);
            var endDate = new DateTime(2022, 07, 30);
            var prices = await provider.GetOilPrices(startDate, endDate);

            foreach (var price in prices)
            {
                var validPrice = price.Price > 0;
                Assert.IsTrue(validPrice);
            }
        }

        [TestMethod]
        public async Task OilPriceProvider_GetOilPrices_EmptyApiUrl_ThrowsException()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(m => m["OilPricesApiUrl"]).Returns("");

            var startDate = new DateTime(2022, 07, 01);
            var endDate = new DateTime(2022, 07, 30);
            var provider = new OilPriceProvider(new HttpClient(), _mapper, mockConfig.Object);
            await Assert.ThrowsExceptionAsync<RestException>(() => provider.GetOilPrices(startDate, endDate));
        }
    }
}