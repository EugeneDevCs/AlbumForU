using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Services
{
    public class LikeService : ILikeService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get; set; }
        public LikeService(IDbAccess db)
        {
            dbAccess = db;
        }
        //here I use automapper to obtain objects from Server layer 
        //and map them to BL models
        public int CountLikes(string pictureId)
        {
            return dbAccess.Likes.GetAll().Where(lk=>lk.PictureId==pictureId).Count();            
        }

        public IEnumerable<LikeBusiness> GetLikes(string pictureId)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Like, LikeBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Like>, List<LikeBusiness>>(dbAccess.Likes.Find(c => c.PictureId == pictureId));
        }

        public bool IsLiked(string userId, string pictureId)
        {
            if(dbAccess.Likes.Find(lk=>lk.PictureId==pictureId && lk.UserId == userId).Count()>0)
            {
                return true;
            }
            return false;
        }

        public void ToLikeDisLike(string userId, string picId)
        {
            Like like = dbAccess.Likes.Find(lk => lk.UserId == userId && lk.PictureId == picId).FirstOrDefault();

            if(like != null)
            {
                dbAccess.Likes.Delete(like.Id);
            }
            else
            {
                dbAccess.Likes.Create(new Like { PictureId=picId, UserId=userId,DateTime=DateTime.Today });
                
            }
            dbAccess.Save();
        }
        public void Dispose()
        {
            dbAccess.Dispose();
        }
    }
}
