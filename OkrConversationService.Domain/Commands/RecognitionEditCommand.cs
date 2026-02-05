using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;


namespace OkrConversationService.Domain.Commands
{
    public class RecognitionEditCommand  : BaseCommand, IRequest<Payload<RecognitionEditRequest>>
    {
        public RecognitionEditRequest RecognitionEditRequest { get; set; }
    }
}
