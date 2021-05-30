using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.BusinessModels
{
    public class PictureBusiness
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserNick { get; set; }
        public string TextBody { get; set; }
        public string PictureId { get; set; }
        public DateTime dateTime { get; set; }
    }
}
