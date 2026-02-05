using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class CheckInCreateCommandHandler : IRequestHandler<CheckInCreateCommand, Payload<CheckInDetailRequest>>
    {
        private readonly ICheckInService _checkInService;
        public CheckInCreateCommandHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<CheckInDetailRequest>> Handle(CheckInCreateCommand request, CancellationToken cancellationToken)
        {
            return await _checkInService.Create(request);
        }
    }
}
