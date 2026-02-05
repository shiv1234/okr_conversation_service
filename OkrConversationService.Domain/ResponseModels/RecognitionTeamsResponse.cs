using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class RecognitionteamsData
    {
        public RecognitionteamsData()
        {
            Employees = new List<RecognitionEmployeeResponse>();
        }
        public int Total { get; set; }
        public List<RecognitionEmployeeResponse> Employees { get; set; }
    }
    public class RecognitionTeamsResponse
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public string BackGroundColorCode { get; set; }
        public string ColorCode { get; set; }
        public string LogoName { get; set; }
        public string ImagePath { get; set; }
        public int TeamMemberCount { get; set; }
        public RecognitionteamsData RecognitionsReceived { get; set; }
        public RecognitionteamsData BadgesReceived { get; set; }
        public RecognitionteamsData RecognitionsGiven { get; set; }

    }
    public class RecognitionEmployeeResponse
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string ImagePath { get; set; }
        public int Total { get; set; }

    }
}
