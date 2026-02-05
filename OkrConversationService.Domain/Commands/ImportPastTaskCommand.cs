using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class ImportPastTaskCommand : BaseCommand, IRequest<Payload<bool>>
    {

    }
}

