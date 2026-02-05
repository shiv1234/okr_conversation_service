using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class DeleteEmployeeCheckInVisibleCommand : BaseCommand, IRequest<Payload<bool>>
    {
        public long EmployeeId { get; set; }
    }
}
