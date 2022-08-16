namespace JsonRpc.Core.DTOs
{
    public abstract class GenericJsonRpcResponse<T>
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public T Result { get; set; }
    }
}