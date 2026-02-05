using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CommentLikeResponse
    {
        public List<long> EmpIds { get; set; } = new List<long>();
        public List<long> TeamId { get; set; } = new List<long>();
      
    }
}
