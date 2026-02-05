using System.Collections.Generic;

namespace OkrConversationService.Domain.ResponseModels
{
    public class MyWallOfFameDashBoardResponse
    {
        public MyWallOfFameDashBoardResponse()
        {
            RecognitionImageMappings = new List<RecognitionImageMappingResponse>();
        }
        public List<RecognitionImageMappingResponse> RecognitionImageMappings { get; set; }

    }
}
