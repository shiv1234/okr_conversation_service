using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;

namespace OkrConversationService.Domain.Commands
{
    public class DraftToPublicUserCommand : BaseCommand, IRequest<Payload<bool>>
    {
        public List<NoteDraftEmailRequest> Goal { get; set; }
    }
}
