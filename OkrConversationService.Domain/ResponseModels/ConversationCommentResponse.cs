using System;
using System.Collections.Generic;


namespace OkrConversationService.Domain.ResponseModels
{
    public class ConversationCommentResponse
    {
        public long ConversationId { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int GoalTypeId { get; set; }
        public long GoalId { get; set; }
        public long GoalSourceId { get; set; }
        public long CreatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsEdited { get; set; }
        public bool IsLiked { get; set; }
        public int TotalLikeCount { get; set; }       
        public List<ConversationReactionResponse> ConversationReactions { get; set; } = new List<ConversationReactionResponse>();
    }
}
