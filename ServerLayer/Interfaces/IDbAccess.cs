﻿using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.Interfaces
{
    public interface IDbAccess
    {
        //entry points to obtain data
        IRepository<AppUser> Users { get;  }
        IRepository<Comment> Comments { get; }
        IRepository<Picture> Pictures { get; }
        IRepository<Thumbnail> Thumbnails { get; }
        IRepository<Topic> Topics { get; }
        IRepository<Like> Likes { get; }

        //for data saving
        void Save();
    }
}
