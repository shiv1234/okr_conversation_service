using System;
using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class RecognitionResponse
    {
        public long RecognitionId { get; set; }
        public string Headlines { get; set; }
        public string Message { get; set; }
        public bool IsAttachment { get; set; }
        public int RecognitionCategoryTypeId { get; set; }
        public string AttachmentImagePath { get; set; }
        public long RecognitionCategoryId { get; set; }
        public long ReceiverId { get; set; }
        public long SenderId { get; set; }
        public bool IsLiked { get; set; } = false;
        public int TotalLikeCount { get; set; } = 0;
        public List<RecognitionLikeResponse> RecognitionLikeResponses { get; set; }
        public bool IsCommented { get; set; } = false;
        public int TotalCommentCount { get; set; } = 0;
        public List<CommentDetailResponse> CommentDetailResponses { get; set; }       
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
