using MediatR;
using OkrConversationService.Domain.ResponseModels;
using System;

namespace OkrConversationService.Domain.Queries
{
    public class GetRecognitionForWallQuery : IRequest<Payload<RecognitionDetailsResponse>>
    {      
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }       
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string emailId { get; set; }
    }
}
