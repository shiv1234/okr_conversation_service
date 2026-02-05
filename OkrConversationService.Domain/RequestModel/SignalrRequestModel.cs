using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class SignalrRequestModel
    {
        public List<long> BroadcastValue { get; set; }
        public string BroadcastTopic { get; set; }
        public long? EmployeeId { get; set; }
    }
}
