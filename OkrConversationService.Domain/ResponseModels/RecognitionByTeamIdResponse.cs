using System;
using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{

    public class RecognitionByTeamIdEmployee
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public string BackGroundColorCode { get; set; }
        public string ColorCode { get; set; }

        public string EmailId { get; set; }
        public string ImagePath { get; set; }

        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
    public class RecognitionByTeamIdResponse
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public List<RecognitionTeam> RecognitionEmployees = new List<RecognitionTeam>();
    }
    public class RecognitionTeam
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string ImagePath { get; set; }
        public int TotalRecognitionsReceived { get; set; }
        public int TotalBadgesReceived { get; set; }
        public int TotalRecognitionsGiven { get; set; }
        public DateTime OrderByDateTime { get; set; }
    }

}
