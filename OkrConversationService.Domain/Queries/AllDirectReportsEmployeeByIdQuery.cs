using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class AllDirectReportsEmployeeByIdQuery : IRequest<Payload<DirectreportsResponseResult>>
    {
        public long EmpId { get; set; }
    }
}
