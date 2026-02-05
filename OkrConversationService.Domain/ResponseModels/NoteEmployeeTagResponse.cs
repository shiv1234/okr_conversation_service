
namespace OkrConversationService.Domain.ResponseModels
{
    public class NoteEmployeeTagResponse
    {
        public long NoteEmployeeTagId { get; set; }
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
    }
}
