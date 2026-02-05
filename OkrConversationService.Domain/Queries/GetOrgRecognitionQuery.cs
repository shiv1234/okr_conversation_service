using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class GetOrgRecognitionQuery : IRequest<Payload<OrgRecognitionResponse>>
    {
        public long Id { get; set; }
        public int SearchType { get; set; }       
        public bool IsMyPost { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long RecognitionId { get; set; }
    }
}
