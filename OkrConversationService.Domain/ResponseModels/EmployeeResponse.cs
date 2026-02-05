using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class EmployeeResponse
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public long EmpId { get; set; }
    }
}
