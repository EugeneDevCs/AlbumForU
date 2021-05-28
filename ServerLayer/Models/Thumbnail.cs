using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServerLayer.Models
{
    public class Thumbnail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string OriginalId { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Path { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string TopicId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string UserId { get; set; }

        public DateTime Date { get; set; }
    }
}
