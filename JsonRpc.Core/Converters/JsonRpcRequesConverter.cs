using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Enums;
using JsonRpc.Core.Errors;

namespace JsonRpc.Core.Converters
{
    public class JsonRpcRequestConverter : JsonConverter<GenericJsonRpcRequest>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(GenericJsonRpcRequest).IsAssignableFrom(typeToConvert);

        public override GenericJsonRpcRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            GenericJsonRpcRequest request;
            var isValid = IsValidJsonRpcRequest(jsonDocument);
            if (isValid)
            {
                var idValue = jsonDocument.RootElement.GetProperty("id").GetInt32();
                var jsonrpcValue = jsonDocument.RootElement.GetProperty("jsonrpc").GetString();
                var methodValue = jsonDocument.RootElement.GetProperty("method").GetString();
                switch (methodValue)
                {
                    case JsonRpcMethodTypes.GetOilPriceTrend:
                        {
                            request = new GetOilPriceTrendRequest
                            {
                                Id = idValue,
                                Jsonrpc = jsonrpcValue,
                                Method = methodValue,
                                Params = GetOilPriceTrendParamsValue(jsonDocument, options)
                            };
                            break;
                        }
                    default:
                        throw new RestException(HttpStatusCode.NotImplemented, new { JsonRpcMethod = $"{methodValue} not implemented" });
                }
            }
            else throw new RestException(HttpStatusCode.BadRequest, new { JsonRpcRequest = $"Not valid" });

            return request;
        }

        public override void Write(Utf8JsonWriter writer, GenericJsonRpcRequest value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        private bool IsValidJsonRpcRequest(JsonDocument jsonDocument)
        {
            var hasMethodProperty = jsonDocument.RootElement.TryGetProperty("id", out var _);
            if (!hasMethodProperty)
                return false;

            var hasIdProperty = jsonDocument.RootElement.TryGetProperty("id", out var _);
            if (!hasIdProperty)
                return false;

            var hasJsonRpcProperty = jsonDocument.RootElement.TryGetProperty("jsonrpc", out var _);
            if (!hasJsonRpcProperty)
                return false;

            return true;
        }

        private GetOilPriceTrendParams GetOilPriceTrendParamsValue(JsonDocument jsonDocument, JsonSerializerOptions options)
        {
            GetOilPriceTrendParams retVal = null;
            try
            {
                var paramsElement = jsonDocument.RootElement.GetProperty("params");
                var deserializedParams = paramsElement.Deserialize<GetOilPriceTrendParams>(options);

                if (deserializedParams.StartDateISO8601.HasValue && deserializedParams.EndDateISO8601.HasValue)
                    retVal = deserializedParams;
                else throw new Exception();
            }
            catch (Exception)
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Params = "Not valid" });
            }
            return retVal;
        }
    }
}