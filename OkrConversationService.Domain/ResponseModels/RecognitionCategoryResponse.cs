using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.ResponseModels
{
    public class RecognitionCategoryResponse
    {
        public long RecognitionCategoryId { get; set; }
        public int RecognitionCategoryTypeId { get; set; }     
        public string Name { get; set; }   
        public bool IsOnlyManager { get; set; }
        public string ImageFilePath { get; set; }
      
    }
}
