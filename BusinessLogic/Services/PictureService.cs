using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using ServerLayer.Repositories;

namespace BusinessLogic.Services
{
    public class PictureService:IPictureService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get;  set; }
        public PictureService(IDbAccess db)
        {
            dbAccess = db;
        }
        //here I use automapper to obtain objects from Server layer 
        //and map them to BL models
        public IEnumerable<PictureBusiness> GetPictures()
        {            
            var mappedData = new MapperConfiguration(config => config.CreateMap<Picture, PictureBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Picture>, List<PictureBusiness>>(dbAccess.Pictures.GetAll());
        }
        public IEnumerable<PictureBusiness> GetCeratainPicture(string oroginalId)
        {            
            var mappedData = new MapperConfiguration(config => config.CreateMap<Picture, PictureBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Picture>, List<PictureBusiness>>(dbAccess.Pictures.Find(p => p.Id == oroginalId));
        }
        public IEnumerable<ThumbnailBusiness> GetThumbs()
        {            
            var mappedData = new MapperConfiguration(config => config.CreateMap<Thumbnail, ThumbnailBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Thumbnail>, List<ThumbnailBusiness>>(dbAccess.Thumbnails.GetAll());
        }
        public IEnumerable<ThumbnailBusiness> GetSearchedThumbs(string searchString)
        {            
            var mappedData = new MapperConfiguration(config => config.CreateMap<Thumbnail, ThumbnailBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Thumbnail>, List<ThumbnailBusiness>>(((ThumbnailRepository)(dbAccess.Thumbnails)).FindSearch(searchString));
        }
       
        public IEnumerable<LikeBusiness> GetLikes(string pictureId)
        {            
            var mappedData = new MapperConfiguration(config => config.CreateMap<Like, LikeBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Like>, List<LikeBusiness>>(dbAccess.Likes.Find(l=>l.PictureId == pictureId));
        }
    }
}
