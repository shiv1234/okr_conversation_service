using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CheckInDatesPermissionResponse
    {
        public List<CheckInWeeklyDatesResponse> CheckInWeeklyDatesResponse { get; set; } = new List<CheckInWeeklyDatesResponse>();
        public bool IsCheckinDataVisible { get; set; }
    }
}
