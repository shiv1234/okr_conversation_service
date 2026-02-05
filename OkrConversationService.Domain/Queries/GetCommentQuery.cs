using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class GetCommentQuery : IRequest<Payload<CommentResponse>>
    {
        public long ModuleDetailsId { get; set; }
        public int ModuleId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
