using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlbumForU.Models
{
    public class Topic
    {

        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Za-z]", ErrorMessage = "Bad tittle, try again")]
        public string Name { get; set; }
    }
}
