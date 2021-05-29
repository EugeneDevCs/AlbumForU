using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServerLayer.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string UserNick { get; set; }
        
        [Column(TypeName ="nvarchar(1000)")]
        [MaxLength(1000)]
        public string TextBody { get; set; }

        [Required]
        public string PictureId { get; set; }
        public DateTime dateTime { get; set; }
    }
}
