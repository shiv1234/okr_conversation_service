using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class ImportPastTaskHandler : IRequestHandler<ImportPastTaskCommand, Payload<bool>>
    {
        private readonly ICheckInService _checkInService;

        public ImportPastTaskHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        public async Task<Payload<bool>> Handle(ImportPastTaskCommand request, CancellationToken cancellationToken)
        {
            return await _checkInService.ImportPastTask(request);
        }
    }
}
