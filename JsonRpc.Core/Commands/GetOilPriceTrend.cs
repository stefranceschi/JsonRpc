using AutoMapper;
using JsonRpc.Core.DTOs;
using JsonRpc.Core.Interfaces;
using MediatR;

namespace JsonRpc.Core.Commands
{
    public class GetOilPriceTrend
    {
        public class Command : IRequest<GenericJsonRpcResponse>
        {
            public GenericJsonRpcRequest JsonRpcRequest { get; set; }
        }

        public class Handler : IRequestHandler<Command, GenericJsonRpcResponse>
        {
            private readonly IOilPriceProvider _priceProvider;
            private readonly IMapper _mapper;
            public Handler(IOilPriceProvider priceProvider, IMapper mapper)
            {
                _priceProvider = priceProvider;
                _mapper = mapper;
            }

            public async Task<GenericJsonRpcResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = _mapper.Map<GenericJsonRpcRequest, GenericJsonRpcResponse>(request.JsonRpcRequest);
                var castedRequest = (GetOilPriceTrendRequest)request.JsonRpcRequest;
                var requestParams = castedRequest.Params;

                var prices = await _priceProvider.GetOilPrices(requestParams.StartDateISO8601, requestParams.EndDateISO8601);
                var result = new GetOilPriceTrendResults { Prices = prices };
                response.Result = result;
                return response;
            }
        }
    }
}