using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.RequestModel
{
    public class ReceiverRequest
    {
        public long Id { get; set; }
        public int SearchType { get; set; }
    }
}
