using Microsoft.AspNetCore.Identity;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.Interfaces
{
    public interface IDbAccess:IDisposable
    {
        //entry points to obtain data
        IRepository<AppUser> Users { get;  }
        IRepository<Comment> Comments { get; }
        IRepository<Picture> Pictures { get; }
        IRepository<Thumbnail> Thumbnails { get; }
        IRepository<Topic> Topics { get; }
        IRepository<Like> Likes { get; }
        IRepository<IdentityRole> Roles { get; }
        IUserRoleRepository UserRoles { get; }

        //for data saving
        void Save();
    }
}
