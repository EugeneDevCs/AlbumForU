using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            IEnumerable<Thumbnail> delThumbs = dbAccess.Thumbnails.Find(th => th.TopicId == id);
            string topicName = dbAccess.Topics.Get(id).Name;
            string directoryPath = "pictures/originals/" + topicName;
            string directoryPathThumb = "pictures/thumbs/" + topicName;
            if (Directory.Exists(webrootPath + "/" + directoryPath))
            {
                Directory.Delete(webrootPath + "/" + directoryPath,true);
            }
            if (Directory.Exists(webrootPath + "/" + directoryPathThumb))
            {
                Directory.Delete(webrootPath + "/" + directoryPathThumb, true);
            }            
            foreach (var pic in delPicture)
            {
                dbAccess.Pictures.Delete(pic.Id);
            }   
            foreach (var thumb in delThumbs)
            {
                dbAccess.Thumbnails.Delete(thumb.Id);
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
        public void Dispose()
        {
            dbAccess.Dispose();
        }
    }
}
