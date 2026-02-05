using OkrConversationService.Domain.Common;
using System;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CheckInWeeklyDatesResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DisplayDate { get; set; }
        public EnumCheckInStatus CheckInStatus { get; set; }
        public string CheckInStatusDetails { get; set; }
        public EnumCheckInWeekType CheckInWeekType { get; set; }
    }
}
