namespace OkrConversationService.Domain.ResponseModels
{
    public class ConversationReactionResponse
    {
        public long LikeReactionId { get; set; }
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public string EmailId { get; set; }
    }
}
