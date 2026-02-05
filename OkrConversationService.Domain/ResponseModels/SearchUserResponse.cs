namespace OkrConversationService.Domain.ResponseModels
{
    public class SearchUserResponse
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string ImagePath { get; set; }
        public string EmailId { get; set; }
        public long? ReportingTo { get; set; }
    }
}
