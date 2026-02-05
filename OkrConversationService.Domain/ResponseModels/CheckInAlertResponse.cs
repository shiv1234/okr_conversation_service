namespace OkrConversationService.Domain.ResponseModels
{
    public class CheckInAlertResponse
    {
        public bool IsAlert { get; set; }
        public string RemainingDays { get; set; }
        public bool IsChangeCheckInVisibilty { get; set; }
        public int CheckInVisibilty { get; set; }
        public bool IsImportButtonVisible { get; set; }
    }
}
