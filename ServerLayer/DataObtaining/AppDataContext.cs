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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Like>()
                .HasKey(o => new { o.UserId, o.PictureId });
        }
        public DbSet<Comment> Comments { get; set; }
    }
}
