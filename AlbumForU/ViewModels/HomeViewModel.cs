using AlbumForU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumForU.ViewModels
{
    public class HomeViewModel
    {
        public List<Topic> Topics { get; set; }
        public List<Thumbnail> ThumbsFirstColumn { get; set; }
        public List<Thumbnail> ThumbsSecondColumn { get; set; }
        public List<Thumbnail> ThumbsThirdColumn { get; set; }
    }
}
