using MediatR;
using Microsoft.AspNetCore.Http;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class UploadFileCommand : BaseCommand, IRequest<Payload<string>>
    {
        public IFormFile FormFile { get; set; }
        public int Type { get; set; }
    }
}
