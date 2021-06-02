using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Repositories;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ServerLayer.Repositories
{
    public class DbAccessRepository : IDbAccess
    {
        //This class is the actually module which allows to obtain the inforamtion from database 
        //via repositories
        private readonly AppDataContext _appData;

        //Here are all repos wich will be sent to business logic layer
        private AppUserRepository appUserRepository;
        private LikeRepository likeRepository;
        private ThumbnailRepository thumbnailRepository;
        private PictureRepository pictureRepository;
        private TopicRepository topicRepository;
        private CommentRepository commentRepository;
        private UserRoleRepository userRoleRepository;
        private RoleRepository roleRepository;
        public DbAccessRepository(AppDataContext data)
        {
            _appData = data;
        }

        public IRepository<AppUser> Users
        {
            get
            {
                if (appUserRepository == null)
                    appUserRepository = new AppUserRepository(_appData);
                return appUserRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(_appData);
                return commentRepository;
            }
        }
        public IRepository<Picture> Pictures
        {
            get
            {
                if (pictureRepository == null)
                    pictureRepository = new PictureRepository(_appData);
                return pictureRepository;
            }
        }
        public IRepository<Thumbnail> Thumbnails
        {
            get
            {
                if (thumbnailRepository == null)
                    thumbnailRepository = new ThumbnailRepository(_appData);
                return thumbnailRepository;
            }
        }

        public IRepository<Topic> Topics
        {
            get
            {
                if (topicRepository == null)
                    topicRepository = new TopicRepository(_appData);
                return topicRepository;
            }
        }

        public IRepository<Like> Likes
        {
            get
            {
                if (likeRepository == null)
                    likeRepository = new LikeRepository(_appData);
                return likeRepository;
            }
        }

        public IUserRoleRepository UserRoles
        {
            get
            {
                if (userRoleRepository == null)
                    userRoleRepository = new UserRoleRepository(_appData);
                return userRoleRepository;
            }
        }

        public IRepository<IdentityRole> Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(_appData);
                return roleRepository;
            }
        }


        public void Save()
        {
            _appData.SaveChanges();
        }


        //These property and methods will help
        //to close data streams
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            //if not disposed already
            if (!this.disposed)
            {
                //and disposing is required
                if (disposing)
                {
                    //dispose stream
                    _appData.Dispose();
                }
                //and mark as DISPOSED
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            //invoke dispose method without parameters
            //which invoke first method
            Dispose(true);

            //this is an important method
            //thate tells CLR not to invoke
            //finilizitaion method for THIS
            //object
            GC.SuppressFinalize(this);
        }
    }
}
