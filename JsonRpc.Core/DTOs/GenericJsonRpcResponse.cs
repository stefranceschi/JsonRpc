namespace JsonRpc.Core.DTOs
{
    public abstract class GenericJsonRpcResponse
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public object Result { get; set; }
    }
}