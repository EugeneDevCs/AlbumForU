using Microsoft.EntityFrameworkCore;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLayer.Repositories
{
    public class TopicRepository : IRepository<Topic>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public TopicRepository(AppDataContext data)
        {
            _appData = data;
        }

        public Topic Create(Topic item)
        {
            return _appData.Topics.Add(item).Entity ;
        }

        public void Delete(string id)
        {
            Topic topic = _appData.Topics.Find(id);
            if (topic != null)
            {
                _appData.Topics.Remove(topic);
            }
        }

        public IEnumerable<Topic> Find(Func<Topic, bool> predicate)
        {
            return _appData.Topics.Where(predicate);
        }

        public Topic Get(string id)
        {
            return _appData.Topics.Find(id);
        }

        public IEnumerable<Topic> GetAll()
        {
            return _appData.Topics;
        }

        public void Update(Topic item)
        {
            _appData.Entry(item).State = EntityState.Modified;
        }
    }
}
