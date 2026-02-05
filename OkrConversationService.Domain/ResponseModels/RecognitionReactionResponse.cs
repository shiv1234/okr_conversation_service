using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class RecognitionReactionResponse
    {
        public bool IsLiked { get; set; }
        public int TotalLikeCount { get; set; }
        public List<RecognitionLikeResponse> RecognitionLikeResponses { get; set; }
    }
}
