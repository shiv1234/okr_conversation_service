

using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class CommentDetailsRequest
    {
        public long CommentDetailsId { get; set; }
        public string Comments { get; set; }       
        public long ModuleDetailsId { get; set; }
        public int ModuleId { get; set; }
        public List<RecognitionImageRequest> RecognitionImageRequests { get; set; }
        public List<RecognitionEmployeeTags> RecognitionEmployeeTags { get; set; } = new List<RecognitionEmployeeTags>();
    }
}
