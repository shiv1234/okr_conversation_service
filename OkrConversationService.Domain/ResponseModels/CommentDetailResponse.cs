using System;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CommentDetailResponse
    {
        public long CommentDetailsId { get; set; }
        public string Comments { get; set; }
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public long ModuleDetailsId { get; set; }
        public int ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsEdited { get; set; }
        public int TotalLikes { get; set; }
        public int TotalReplies { get; set; }
        public bool IsLiked { get; set; }
        public string ReplyFirstName { get; set; }
        public string ReplyLastName { get; set; }
        public string ReplyFullName { get; set; }
        public string ReplyImagePath { get; set; }
    }
}
