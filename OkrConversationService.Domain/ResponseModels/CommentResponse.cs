using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CommentResponse
    {
        public int TotalComments { get; set; }  
        public List<CommentDetailResponse> CommentDetailResponses { get; set; }
    }
}
