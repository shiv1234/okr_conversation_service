namespace OkrConversationService.Domain.RequestModel
{
    public class UserNotificationsAndEmailsRequest
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string EmailId { get; set; }
        public string EmployeeCode { get; set; }
    }
}
