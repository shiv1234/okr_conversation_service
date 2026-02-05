using MediatR;
using OkrConversationService.Domain.ResponseModels;


namespace OkrConversationService.Domain.Queries
{
    public class RecognitionCategoryGetQuery : IRequest<Payload<RecognitionCategoryResponse>>
    {
       public long EmployeeId { get; set; }
    }
}
