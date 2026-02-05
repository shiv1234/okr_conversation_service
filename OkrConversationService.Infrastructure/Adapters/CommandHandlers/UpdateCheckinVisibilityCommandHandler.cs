using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class UpdateCheckinVisibilityCommandHandler : IRequestHandler<UpdateCheckinVisibilityCommand, Payload<CheckInVisible>>
    {
        private readonly ICheckInService _checkInService;
        public UpdateCheckinVisibilityCommandHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<CheckInVisible>> Handle(UpdateCheckinVisibilityCommand request, CancellationToken cancellationToken)
        {
            return await _checkInService.UpdateCheckinVisibility(request);
        }
    }
}
