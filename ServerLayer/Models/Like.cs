using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServerLayer.Models
{
    public class Like
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        
        [Key]
        [Column(Order = 2)]
        public string PictureId { get; set; }
        
        public DateTime DateTime { get; set; }


    }
}
