using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class GetRecognitionByIdQuery : IRequest<Payload<RecognitionResponse>>
    {
        public long RecognitionId { get; set; }
    }
}
