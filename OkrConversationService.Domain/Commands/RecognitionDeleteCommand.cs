using MediatR;
using OkrConversationService.Domain.ResponseModels;


namespace OkrConversationService.Domain.Commands
{
    public class RecognitionDeleteCommand : BaseCommand, IRequest<Payload<bool>>
    {
        public long RecognitionId { get; set; }
    }
}
