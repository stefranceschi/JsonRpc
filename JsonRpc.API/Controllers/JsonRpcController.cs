using JsonRpc.Core.Commands;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Enums;
using JsonRpc.Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace JsonRpc.API.Controllers
{
    public class JsonRpcController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<GenericJsonRpcResponse>> ExecuteCommand(GenericJsonRpcRequest jsonRpcRequest)
        {
            GenericJsonRpcResponse response;
            switch (jsonRpcRequest.Method)
            {
                case JsonRpcMethodTypes.GetOilPriceTrend:
                    response = await Mediator.Send(new GetOilPriceTrend.Command { JsonRpcRequest = jsonRpcRequest });
                    break;
                default:
                    throw new RestException(System.Net.HttpStatusCode.NotImplemented, new { JsonRpcMethod = $"{jsonRpcRequest.Method} not implemented" });
            }
            return response;
        }
    }
}