using JsonRpc.Core.Commands;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JsonRpc.API.Controllers
{
    public class JsonRpcController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> ExecuteCommand(GenericJsonRpcRequest jsonRpcRequest)
        {
            switch (jsonRpcRequest.Method)
            {
                case JsonRpcMethodTypes.GetOilPriceTrend:
                    await Mediator.Send(new GetOilPriceTrend.Command());
                    break;
                default:
                    break;
            } 
            return Unit.Value;
        }
    }
}