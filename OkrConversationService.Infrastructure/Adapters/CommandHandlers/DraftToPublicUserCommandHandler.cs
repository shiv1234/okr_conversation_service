using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    class DraftToPublicUserCommandHandler : IRequestHandler<DraftToPublicUserCommand, Payload<bool>>
    {
        private readonly INoteService _noteService;

        public DraftToPublicUserCommandHandler(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<Payload<bool>> Handle(DraftToPublicUserCommand request, CancellationToken cancellationToken)
        {
            return await _noteService.DraftToPublicUserNotificationsAndEmails(request.Goal);
        }
    }
}
