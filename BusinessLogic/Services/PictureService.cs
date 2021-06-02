using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using ServerLayer.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BusinessLogic.Services
{
    public class PictureService : IPictureService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get; set; }
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
        public PictureBusiness GetCeratainPicture(string oroginalId)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Picture, PictureBusiness>()).CreateMapper();
            return mappedData.Map<Picture, PictureBusiness>(dbAccess.Pictures.Get(oroginalId));
        }
        public IEnumerable<ThumbnailBusiness> GetThumbs(int page)
        {
            int indexFirst = 9 * (page - 1);
            int indexLast = 9 * page +1;

            var mappedData = new MapperConfiguration(config => config.CreateMap<Thumbnail, ThumbnailBusiness>()).CreateMapper();
            List<ThumbnailBusiness> thumbns = mappedData.Map<IEnumerable<Thumbnail>, List<ThumbnailBusiness>>(dbAccess.Thumbnails.GetAll());
            return new List<ThumbnailBusiness>(from thum in thumbns.Where((s, i) => i >= indexFirst && i < indexLast)
                                               select thum);
        }
        public IEnumerable<ThumbnailBusiness> GetFilteredByTopicThumbs(string topicId, int page)
        {
            int indexFirst = 9 * (page - 1);
            int indexLast = 9 * page + 1;

            var mappedData = new MapperConfiguration(config => config.CreateMap<Thumbnail, ThumbnailBusiness>()).CreateMapper();
            List<ThumbnailBusiness> thumbns=mappedData.Map<IEnumerable<Thumbnail>, List<ThumbnailBusiness>>(dbAccess.Thumbnails.Find(th=>th.TopicId== topicId));
            return new List<ThumbnailBusiness>(from thum in thumbns.Where((s, i) => i >= indexFirst && i < indexLast)
                                               select thum);
        }
        public IEnumerable<ThumbnailBusiness> GetSearchedThumbs(string searchString)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Thumbnail, ThumbnailBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Thumbnail>, List<ThumbnailBusiness>>(((ThumbnailRepository)(dbAccess.Thumbnails)).FindSearch(searchString));
        }
        public IEnumerable<ThumbnailBusiness> GetUserThumbs(string userId)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Thumbnail, ThumbnailBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Thumbnail>, List<ThumbnailBusiness>>(dbAccess.Thumbnails.Find(th=>th.UserId== userId));
        }

        public async Task AddPicture(string PictureName, string TopicId, IFormFile Picture, string webrootPath, string currentUserID)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Topic, TopicBusiness>()).CreateMapper();
            string topicName = mappedData.Map<Topic, TopicBusiness>((dbAccess.Topics.Get(TopicId))).Name;
             
            //Check if the directory exists
            string directoryPath = "pictures/originals/" + topicName;
            if (!Directory.Exists(webrootPath + "/" + directoryPath))
            {
                Directory.CreateDirectory(webrootPath + "/" + directoryPath);
            }

            string directoryPathThumb = "pictures/thumbs/" + topicName;
            if (!Directory.Exists(webrootPath + "/" + directoryPathThumb))
            {
                Directory.CreateDirectory(webrootPath + "/" + directoryPathThumb);
            }

            string picturePath = directoryPath + "/" + Picture.FileName;
            string picturePaththumb = directoryPathThumb + "/" + Picture.FileName;

            using (var image = Image.Load(Picture.OpenReadStream()))
            {
                int imageWidth = image.Width - (image.Width - 500);// now width = 500 px

                int percents = imageWidth * 100 / image.Width; // now we know how many percents
                                                               //we should subtract
                int imageHeight = percents * image.Height / 100;//and now we have the image height

                image.Mutate(x => x.Resize(imageWidth, imageHeight));
                image.SaveAsJpeg(webrootPath + "/" + picturePaththumb);
            }

            //Saving picture to our file system
            using (var fileStream = new FileStream(webrootPath + "/" + picturePath, FileMode.CreateNew))
            {
                await Picture.CopyToAsync(fileStream);
            }

            //Check if the directory foe thumb exists
            

            //Saving resized (width = 500px) thumb to our file system
            
            string origId = dbAccess.Pictures.Create(new Picture() { Path = picturePath, Name = PictureName, TopicId = TopicId, UserId = currentUserID, Date = DateTime.Today }).Id;
            string thumbId= dbAccess.Thumbnails.Create(new Thumbnail() { Path = picturePaththumb, OriginalId = origId, TopicId = TopicId, UserId = currentUserID, Date = DateTime.Today }).Id;
            dbAccess.Save();
        }

        public void Delete(string id, string webrootPath)
        {
            Picture delPic = dbAccess.Pictures.Get(id);
            Thumbnail delThumb = dbAccess.Thumbnails.Find(th => th.OriginalId == id).FirstOrDefault();
            IEnumerable<Comment> delComments = dbAccess.Comments.Find(th => th.PictureId == id);
            IEnumerable<Like> delLikes = dbAccess.Likes.Find(lk => lk.PictureId == id);


            if (System.IO.File.Exists(webrootPath +"/"+ delPic.Path))
            {
                System.IO.File.Delete(webrootPath + "/" + delPic.Path);
            }

            if (System.IO.File.Exists(webrootPath +"/"+ delThumb.Path))
            {
                System.IO.File.Delete(webrootPath + "/" + delThumb.Path);
            }

            

            foreach (var comment in delComments)
            {
                dbAccess.Comments.Delete(comment.Id);
            }
            foreach (var like in delLikes)
            {
                dbAccess.Likes.Delete(like.Id);
            }

            dbAccess.Thumbnails.Delete(delThumb.Id);
            dbAccess.Pictures.Delete(id);

            dbAccess.Save();
        }

        public void Update(PictureBusiness picture)
        {
            Picture original = dbAccess.Pictures.Get(picture.Id);
            if (original.Name != picture.Name)
            {
                var mappedData = new MapperConfiguration(config => config.CreateMap<PictureBusiness, Picture>()).CreateMapper();
                Picture updatePicture = mappedData.Map<PictureBusiness, Picture>(picture);
                dbAccess.Pictures.Update(updatePicture);
                dbAccess.Save();
            }
        }

    }
}
