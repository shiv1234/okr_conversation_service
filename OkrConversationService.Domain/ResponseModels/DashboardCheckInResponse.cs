using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class DashboardCheckInResponse
    {
        public bool IsAlert { get; set; }
        public string RemainingDays { get; set; }
        public List<ObjectStatus> CheckInStatus { get; set; }
        public int TaskCount { get; set; }
        public string DisplayDate { get; set; }
        public double RemaingDaysLeft { get; set; }
        public bool IsCheckInSubmitted { get; set; }

    }
}
