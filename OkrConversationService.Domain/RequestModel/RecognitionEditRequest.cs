using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class RecognitionEditRequest
    {
        public long RecognitionId { get; set; }      
        public string Message { get; set; }
        public bool IsAttachment { get; set; }   
        public long RecognitionCategoryId { get; set; }
        public int RecognitionCategoryTypeId { get; set; }
        public bool IsContentChange { get; set; }
        public List<RecognitionImageRequest> RecognitionImageRequests { get; set; }
        public List<RecognitionEmployeeTags> RecognitionEmployeeTags { get; set; } = new List<RecognitionEmployeeTags>();
        public List<ReceiverRequest> ReceiverRequest { get; set; } = new List<ReceiverRequest>();
    }
}
