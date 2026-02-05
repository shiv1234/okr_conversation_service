using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class IsCheckInSubmittedQuery : IRequest<Payload<CheckInAlertResponse>>
    {

    }
}
