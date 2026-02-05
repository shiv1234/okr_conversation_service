using System;

namespace OkrConversationService.Domain.RequestModel
{
    public class RecognitionForWallRequest
    {
       
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }       
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;      
        public string emailId { get; set; }
    }
}
