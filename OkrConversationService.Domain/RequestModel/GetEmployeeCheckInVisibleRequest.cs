using OkrConversationService.Domain.Common;
using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class GetEmployeeCheckInVisibleRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortingText { get; set; } = AppConstants.DefaultCheckInVisibleSorting;
        public string Order { get; set; } = "asc";
        public List<string> Finder { get; set; }
    }
}
