using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.RequestModel
{
    public class RecognitionRequest
    {
        public long empId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool isMyPost { get; set; }
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public long RecognitionId { get; set; }
    }
}
