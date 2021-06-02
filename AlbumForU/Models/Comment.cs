using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlbumForU.Models
{
    public class Comment
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string UserNick { get; set; }
        public string TextBody { get; set; }
        public string PictureId { get; set; }
        public DateTime dateTime { get; set; }
    }
}
