using System;
using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class EmployeesLeaderBoardRequest
    {
    
        public long Id { get; set; }
        public int SearchType { get; set; }
    }

    public class RecognitionFilter
    {
        public long RecognitionId { get; set; }
        public long ReceiverId { get; set; }
        public long TeamId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsAttachment { get; set; }
        public int RecognitionCategoryTypeId { get; set; }
        public long CreatedBy { get; set; }
    }
    public class RecognitionGroupFilter : RecognitionFilter
    {

        public List<RecognitionFilter> Items { get; set; }
        public RecognitionGroupFilter()
        {
            Items = new List<RecognitionFilter>();
        }

    }

    public class RecognitionGroupFilterWallofFame
    {
        public long RecognitionId { get; set; }


    }


    public partial class RecognitionWallOfFame
    {

        public long RecognitionId { get; set; }
        public long ReceiverId { get; set; } = 0;
        public string Headlines { get; set; } = "";
        public string Message { get; set; }
        public bool IsAttachment { get; set; }
        public bool IsGivenByManager { get; set; }
        public int RecognitionCategoryTypeId { get; set; }
        public bool IsActive { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
