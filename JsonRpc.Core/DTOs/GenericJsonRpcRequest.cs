using System.Text.Json.Serialization;
using JsonRpc.Core.Converters;

namespace JsonRpc.Core.DTOs
{
    [JsonConverter(typeof(JsonRpcRequestConverter))]
    public abstract class GenericJsonRpcRequest
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public string Method { get; set; }
    }
}