using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.BusinessModels
{
    public class ThumbnailBusiness
    {
        public string Id { get; set; }
        public string OriginalId { get; set; }
        public string Path { get; set; }
        public string TopicId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
