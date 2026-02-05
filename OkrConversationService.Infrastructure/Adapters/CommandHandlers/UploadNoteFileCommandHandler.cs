using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class UploadNoteFileCommandHandler : IRequestHandler<UploadFileCommand, Payload<string>>
    {
        private readonly INoteService _noteService;

        public UploadNoteFileCommandHandler(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<Payload<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _noteService.UploadNotesImageOnAzure(request);
        }
    }
}
