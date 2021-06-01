using AlbumForU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumForU.ViewModels
{
    public class PictureViewModel
    {
        public Picture Picture { get; set; }
        public List<Comment> Comments { get; set; }
        public int QuantityLikes { get; set; }
        public bool IsLiked { get; set; }
        public AppUser appUser{get;set;}
    }
}
