using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class RecognitionDetailsResponse
    {
        public long RecognitionId { get; set; }
        public string Headlines { get; set; }
        public string Message { get; set; }
        public bool IsAttachment { get; set; }
        public int RecognitionCategoryTypeId { get; set; }
        public string AttachmentImagePath { get; set; }
        public string AttachmentName { get; set; }
        public long RecognitionCategoryId { get; set; }
        public long ReceiverId { get; set; }
        public long SenderId { get; set; }
        public bool IsLiked { get; set; } = false;
        public int TotalLikeCount { get; set; } = 0;
        public List<RecognitionLikeResponse> RecognitionLikeResponses { get; set; }
        public bool IsCommented { get; set; } = false;
        public int TotalCommentCount { get; set; } = 0;
        public bool IsEditable { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderFullName { get; set; }
        public string SenderEmailId { get; set; }
        public long SenderEmployeeId { get; set; }
        public string SenderImagePath { get; set; }
        public bool IsEdited { get; set; }
        public List<ReceiverDetail> receiverDetails { get; set; } = new List<ReceiverDetail>();
        public List<CommentDetailResponse> CommentDetailResponses { get; set; } = new List<CommentDetailResponse>();
    }
}
