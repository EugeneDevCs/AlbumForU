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
    public class TopicService : ITopicService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get; set; }
        public TopicService(IDbAccess db)
        {
            dbAccess = db;
        }
        //here I use automapper to obtain objects from Server layer 
        //and map them to BL models
        public IEnumerable<TopicBusiness> GetTopics()
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Topic, TopicBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Topic>, List<TopicBusiness>>(dbAccess.Topics.GetAll());
           
        }

        public TopicBusiness GetCeratainTopic(string topicId)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Topic, TopicBusiness>()).CreateMapper();
            return mappedData.Map<Topic, TopicBusiness>(dbAccess.Topics.Get(topicId));
        }
        public void Delete(string id, string webrootPath)
        {
            
            IEnumerable<Picture> delPicture = dbAccess.Pictures.Find(th => th.TopicId == id);
            
            foreach (var pic in delPicture)
            {
                DeleteWhithoutPictureSaving(pic.Id, webrootPath);
            }        
            
            dbAccess.Topics.Delete(id);

            dbAccess.Save();
        }

        public void Create(string name)
        {
            dbAccess.Topics.Create(new Topic { Name = name });
            dbAccess.Save();
        }
        public void Update(TopicBusiness topic)
        {
            Topic original =dbAccess.Topics.Get(topic.Id);
            if(original.Name!=topic.Name)
            {
                var mappedData = new MapperConfiguration(config => config.CreateMap<TopicBusiness, Topic>()).CreateMapper();
                Topic updateTopic = mappedData.Map<TopicBusiness, Topic>(topic);
                dbAccess.Topics.Update(updateTopic);
                dbAccess.Save();
            }
                        
        }


        private void DeleteWhithoutPictureSaving(string id, string webrootPath)
        {
            Picture delPic = dbAccess.Pictures.Get(id);
            Thumbnail delThumb = dbAccess.Thumbnails.Find(th => th.OriginalId == id).FirstOrDefault();
            IEnumerable<Comment> delComments = dbAccess.Comments.Find(th => th.PictureId == id);
            IEnumerable<Like> delLikes = dbAccess.Likes.Find(lk => lk.PictureId == id);


            if (System.IO.File.Exists(webrootPath + "/" + delPic.Path))
            {
                System.IO.File.Delete(webrootPath + "/" + delPic.Path);
            }

            if (System.IO.File.Exists(webrootPath + "/" + delThumb.Path))
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
        }

    }
}
