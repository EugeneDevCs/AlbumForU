using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
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
        public void Delete(string id)
        {
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
            var mappedData = new MapperConfiguration(config => config.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            Topic updateTopic = mappedData.Map<TopicBusiness, Topic>(topic);
            dbAccess.Topics.Update(updateTopic);
            dbAccess.Save();
        }
    }
}
