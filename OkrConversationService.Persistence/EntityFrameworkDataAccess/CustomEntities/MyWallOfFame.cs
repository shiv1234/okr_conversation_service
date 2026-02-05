using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class MyWallOfFame
    {
        public MyWallOfFame()
        {
            RecognitionEmployeeTeamMapping = new RecognitionEmployeeTeamMapping();
            Recognition = new Recognition();
            RecognitionImageMapping = new RecognitionImageMapping();
            Team = new Team();

        }
        public bool IsTeamBadge { get; set; }
        public Recognition Recognition { get; set; }
        public RecognitionImageMapping RecognitionImageMapping { get; set; }
        public RecognitionEmployeeTeamMapping RecognitionEmployeeTeamMapping { get; set; }
        public Team Team { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class MyWallOfFameTeam
    {
        public MyWallOfFameTeam()
        {
            RecognitionEmployeeTeamMappings = new List<RecognitionEmployeeTeamMapping>();
            Recognitions = new List<Recognition>();
            RecognitionImageMappings = new List<RecognitionImageMapping>();
            Teams = new List<Team>();

        }
        public long Id { get; set; }
        public int SearchType { get; set; }
        public bool IsTeamBadge { get; set; }
        public List<Recognition> Recognitions { get; set; }
        public List<RecognitionImageMapping> RecognitionImageMappings { get; set; }
        public List<RecognitionEmployeeTeamMapping> RecognitionEmployeeTeamMappings { get; set; }
        public List<Team> Teams { get; set; }
    }
}
