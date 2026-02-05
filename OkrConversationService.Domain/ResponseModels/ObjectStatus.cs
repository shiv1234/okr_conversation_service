using System;

namespace OkrConversationService.Domain.ResponseModels
{
    public class ObjectStatus
    {
        public int CheckInStatusId { get; set; }
        public string CheckInStatus { get; set; }     
        public string DisplayDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
