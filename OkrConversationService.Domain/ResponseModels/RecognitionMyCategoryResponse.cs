using System.Collections.Generic;
namespace OkrConversationService.Domain.ResponseModels
{
    public class MyWallOfFameResponse
    {

        public string GivenByText { get; set; }

        public MyWallOfFameResponse()
        {
            RecognitionImageMappings = new List<RecognitionImageMappingResponse>();
        }
        public List<RecognitionImageMappingResponse> RecognitionImageMappings { get; set; }

    }

    public class RecognitionImageMappingResponse
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string GuidFileName { get; set; }
        public string ImageFilePath { get; set; }
        public int TotalCount { get; set; }
        public long CreatedBy { get; set; }
        public bool IsNewBadge { get; set; }
        public RecognitionImageMappingResponse()
        {
            RecognitionUserDetails = new List<RecognitionUserDetailsResponse>();
        }
        public List<RecognitionUserDetailsResponse> RecognitionUserDetails { get; set; }
    }
    public class RecognitionUserDetailsResponse
    {
        public long EmployeeId { get; set; }
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string Designation { get; set; }
        public int Count { get; set; }


        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public string LogoName { get; set; }
        public string LogoImagePath { get; set; }
        public string BackGroundColorCode { get; set; }
        public string ColorCode { get; set; }
    }

    public class RecognitionTeamDetailResponse
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public string LogoName { get; set; }
        public string LogoImagePath { get; set; }
        public string BackGroundColorCode { get; set; }
        public string ColorCode { get; set; }
        public int Count { get; set; }

    }

}
