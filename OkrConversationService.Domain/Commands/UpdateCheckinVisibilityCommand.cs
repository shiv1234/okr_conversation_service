using MediatR;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class UpdateCheckinVisibilityCommand : BaseCommand, IRequest<Payload<CheckInVisible>>
    {
        public CheckInVisible CheckInVisibilty { get; set; }
    }
}
