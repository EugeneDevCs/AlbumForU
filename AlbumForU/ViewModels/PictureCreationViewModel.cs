using Microsoft.AspNetCore.Http;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumForU.ViewModels
{
    public class PictureCreationViewModel
    {
        [Required]
        public string PictureName { get; set; }
        [Required]
        public string TopicId { get; set; }
        [Required]
        public IFormFile Picture { get; set; }

        public List<Topic> Topics { get; set; }
    }
}
