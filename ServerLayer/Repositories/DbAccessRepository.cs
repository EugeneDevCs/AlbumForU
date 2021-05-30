using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Repositories;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        public DbAccessRepository(AppDataContext data)
        {
            _appData = data;
        }

        public IRepository<AppUser> Users => throw new NotImplementedException();

        public IRepository<Comment> Comments => throw new NotImplementedException();

        public IRepository<Picture> Pictures => throw new NotImplementedException();

        public IRepository<Thumbnail> Thumbnails => throw new NotImplementedException();

        public IRepository<Topic> Topics => throw new NotImplementedException();

        public IRepository<Like> Likes => throw new NotImplementedException();

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
