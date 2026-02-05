namespace OkrConversationService.Domain.ResponseModels
{
    public class TeamByEmpIdResponse
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public string Colorcode { get; set; }
        public string BackGroundColorCode { get; set; }
    }
}
