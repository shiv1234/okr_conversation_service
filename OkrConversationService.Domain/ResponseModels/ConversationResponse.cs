using System;
using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class ConversationResponse
    {
        public ConversationResponse()
        {
            ConversationReactions = new List<ConversationReactionResponse>();
        }
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
        public int TotalReplies { get; set; }
        public string ReplyFirstName { get; set; }
        public string ReplyLastName { get; set; }
        public string ReplyFullName { get; set; }
        public string ReplyImagePath { get; set; }
        public List<ConversationReactionResponse> ConversationReactions { get; set; }
    }
}
