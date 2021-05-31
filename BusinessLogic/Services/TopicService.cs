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
    }
}
