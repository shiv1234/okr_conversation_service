using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class DirectReportsResponse
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string ImagePath { get; set; }
        public List<ObjectStatus> CheckInStatus { get; set; }


    }

    public class DirectreportsResponseResult
    {
        public List<DirectReportsResponse> DirectReports { get; set; }
        public List<DirectReportsResponse> OtherReports { get; set; }
    }
}
