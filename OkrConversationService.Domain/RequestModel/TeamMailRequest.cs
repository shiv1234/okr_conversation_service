using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace OkrConversationService.Domain.RequestModel
{
    public class TeamMailRequest 
    {
        public List<EmployeeResponse> Employees { get; set; }
        public string MailTo { get; set; }
        public string MailFrom { get; set; } = "";
        public string Bcc { get; set; } = "";
        public string Cc { get; set; } = "";
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailBody { get; set; }
    }
}
