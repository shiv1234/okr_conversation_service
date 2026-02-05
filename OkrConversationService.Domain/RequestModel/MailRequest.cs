namespace OkrConversationService.Domain.RequestModel
{
    public class MailRequest
    {
        public string MailTo { get; set; }
        public string MailFrom { get; set; } = "";
        public string Bcc { get; set; } = "";
        public string Cc { get; set; } = "";
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
