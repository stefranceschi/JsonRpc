using System.Text.Json;
using JsonRpc.Core.Converters;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Enums;
using JsonRpc.Core.Errors;

namespace JsonRpc.Tests
{
    [TestClass]
    public class SerializationTests
    {
        private readonly JsonSerializerOptions serializeOptions;
        public SerializationTests()
        {
            serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonRpcRequestConverter()
                }
            };
        }


        [TestMethod]
        public void Deserealize_Well_Formatted_Json()
        {
            var json = @"{
                        ""id"": 1,
                        ""jsonrpc"": ""2.0"",
                        ""method"": ""GetOilPriceTrend"",
                        ""params"": {
                        ""startDateISO8601"": ""2020-01-01"",
                        ""endDateISO8601"": ""2020-01-05""
                        }
                        }";

            GenericJsonRpcRequest deserialized = JsonSerializer.Deserialize<GenericJsonRpcRequest>(json, serializeOptions);
            Assert.IsNotNull(deserialized);
            Assert.IsNotNull(deserialized is GetOilPriceTrendRequest);
            Assert.AreEqual(deserialized.Id, 1);
            Assert.AreEqual(deserialized.Jsonrpc, "2.0");
            Assert.AreEqual(deserialized.Method, JsonRpcMethodTypes.GetOilPriceTrend);
        }

        [TestMethod]
        public void Deserealize_Missing_Id_Throws_Exception()
        {
            var json = @"{
                        ""jsonrpc"": ""2.0"",
                        ""method"": ""GetOilPriceTrend"",
                        ""params"": {
                        ""startDateISO8601"": ""2020-01-01"",
                        ""endDateISO8601"": ""2020-01-05""
                        }
                        }";

            Assert.ThrowsException<RestException>(() => JsonSerializer.Deserialize<GenericJsonRpcRequest>(json, serializeOptions));
        }

        [TestMethod]
        public void Deserealize_Missing_Jsonrpc_Throws_Exception()
        {
            var json = @"{
                        ""id"": 1,
                        ""method"": ""GetOilPriceTrend"",
                        ""params"": {
                        ""startDateISO8601"": ""2020-01-01"",
                        ""endDateISO8601"": ""2020-01-05""
                        }
                        }";

            Assert.ThrowsException<RestException>(() => JsonSerializer.Deserialize<GenericJsonRpcRequest>(json, serializeOptions));
        }

        [TestMethod]
        public void Deserealize_Missing_WrongMethod_Throws_Exception()
        {
            var json = @"{
                        ""id"": 1,
                        ""jsonrpc"": ""2.0"",
                        ""method"": ""NotImplementedMethod"",
                        ""params"": {
                        ""startDateISO8601"": ""2020-01-01"",
                        ""endDateISO8601"": ""2020-01-05""
                        }
                        }";

            Assert.ThrowsException<RestException>(() => JsonSerializer.Deserialize<GenericJsonRpcRequest>(json, serializeOptions));
        }

        [TestMethod]
        public void Deserealize_Missing_WrongParams_Throws_Exception()
        {
            var json = @"{
                        ""id"": 1,
                        ""jsonrpc"": ""2.0"",
                        ""method"": ""GetOilPriceTrend"",
                        ""params"": {
                        ""startDateISO8601"": ""2020-01-01"",
                        ""endDateISO8601"": ""2020-01-05""
                        }
                        }";

            Assert.ThrowsException<RestException>(() => JsonSerializer.Deserialize<GenericJsonRpcRequest>(json, serializeOptions));
        }
    }
}