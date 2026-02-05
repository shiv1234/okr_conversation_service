using System;

namespace OkrConversationService.Domain.RequestModel
{
    public class MyWallOfFameRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long Id { get; set; }
        public int SearchType { get; set; }
      
    }
}
