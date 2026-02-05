using System;

namespace OkrConversationService.Domain.ResponseModels
{
    public class CheckInDetailsResponse
    {
        public int CheckInPointsId { get; set; }
        public string CheckInPoints { get; set; }
        public long CheckInDetailsId { get; set; }
        public string CheckInDetails { get; set; }
        public DateTime CheckInDate { get; set; }
        public bool IsAfterCycleCutoff { get; set; }
        public DateTime? CheckInSubmitDate { get; set; }
    }
}
