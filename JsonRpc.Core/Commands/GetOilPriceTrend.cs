using MediatR;

namespace JsonRpc.Core.Commands
{
    public class GetOilPriceTrend
    {
        public class Command : IRequest<Unit>
        {

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            public Handler()
            {

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                return Unit.Value;
            }
        }
    }
}