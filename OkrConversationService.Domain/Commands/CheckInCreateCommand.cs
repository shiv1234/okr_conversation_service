using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;

namespace OkrConversationService.Domain.Commands
{
    public class CheckInCreateCommand : BaseCommand, IRequest<Payload<CheckInDetailRequest>>
    {
        public List<CheckInDetailRequest> CheckInDetailRequest { get; set; }
    }
}
