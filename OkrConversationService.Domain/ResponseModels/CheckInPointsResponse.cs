using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CheckInPointsResponse
    {
        public bool IsOldVersion { get; set; }
        public List<CheckInDetailsResponse> CheckInDetailsResponse { get; set; } = new List<CheckInDetailsResponse>();
        public List<TaskResponse> TaskResponse { get; set; } = new List<TaskResponse>();
    }
}