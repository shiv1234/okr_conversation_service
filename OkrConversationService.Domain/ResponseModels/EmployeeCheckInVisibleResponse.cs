using System;
using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class EmployeeCheckInVisibleResponse
    {
        public List<EmployeeCheckInVisible> EmployeeCheckInVisibles { get; set; } = new List<EmployeeCheckInVisible>();
        public int CheckInVisibilty { get; set; }
    }
    public class EmployeeCheckInVisible
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string EmailId { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReportingManger { get; set; }

    }
}
