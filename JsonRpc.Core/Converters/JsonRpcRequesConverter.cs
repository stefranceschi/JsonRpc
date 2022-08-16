using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Enums;

namespace JsonRpc.Core.Converters
{
    public class JsonRpcRequestConverter : JsonConverter<GenericJsonRpcRequest>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(GenericJsonRpcRequest).IsAssignableFrom(typeToConvert);

        public override GenericJsonRpcRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            GenericJsonRpcRequest request;
            JsonElement jsonElement;
            var hasMethodProperty = jsonDocument.RootElement.TryGetProperty("method", out jsonElement);
            if (hasMethodProperty)
            {
                var idValue = jsonDocument.RootElement.GetProperty("id").GetInt32();
                var jsonrpcValue = jsonDocument.RootElement.GetProperty("jsonrpc").GetString();
                var methodValue = jsonDocument.RootElement.GetProperty("method").GetString();
                switch (methodValue)
                {
                    case JsonRpcMethodTypes.GetOilPriceTrend:
                        {

                            var paramsElement = jsonDocument.RootElement.GetProperty("params");
                            var deserializedParams = paramsElement.Deserialize<GetOilPriceTrendParams>(options);
                            request = new GetOilPriceTrendRequest
                            {
                                Id = idValue,
                                Jsonrpc = jsonrpcValue,
                                Method = methodValue,
                                Params = deserializedParams
                            };
                            break;
                        }
                    default:
                        throw new NotImplementedException(string.Format("{methodValue} not implemented"));
                }
            }
            else throw new ArgumentException("Missing method", "method");

            return request;
        }

        public override void Write(Utf8JsonWriter writer, GenericJsonRpcRequest value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        private void SetGetOilPriceTrendParams(JsonElement jsonElement, GenericJsonRpcRequest genericRequest)
        {

        }
    }
}