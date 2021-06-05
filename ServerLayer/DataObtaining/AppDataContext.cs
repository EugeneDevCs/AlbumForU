using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ServerLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.DataObtaining
{
    public class AppDataContext:IdentityDbContext<AppUser>
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options){}

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Thumbnail>Thumbs { get; set; }
    }
}
