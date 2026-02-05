using System;
namespace OkrConversationService.Domain.RequestModel
{
    public class RecognitionTeamRequest
    {
        public long TeamId { get; set; }      
        public long Id { get; set; }
        public int SearchType { get; set; }
    }
}
