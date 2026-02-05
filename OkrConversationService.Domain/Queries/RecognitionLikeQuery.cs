using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class RecognitionLikeQuery : IRequest<Payload<RecognitionReactionResponse>>
    {
        public long ModuleDetailsId { get; set; }
        public int ModuleId { get; set; }
       
    }
}
