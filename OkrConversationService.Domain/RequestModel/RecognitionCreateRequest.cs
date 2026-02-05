using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.RequestModel
{
    public class RecognitionCreateRequest
    {
        public long RecognitionId { get; set; }
        public string Message { get; set; }
        public bool IsAttachment { get; set; }
        /// <summary>
        /// RecognitionCategoryTypeId = 1 - Badge  
        /// </summary>
        public int RecognitionCategoryTypeId { get; set; }
        /// <summary>
        /// RecognitionCategoryId = badges master table ID
        /// </summary>
        public long RecognitionCategoryId { get; set; }
        public List<RecognitionImageRequest> RecognitionImageRequests { get; set; } = new List<RecognitionImageRequest>();
        public List<RecognitionEmployeeTags> RecognitionEmployeeTags { get; set; } = new List<RecognitionEmployeeTags>();
        public List<ReceiverRequest> ReceiverRequest { get; set; } = new List<ReceiverRequest>();
       

    }

}
