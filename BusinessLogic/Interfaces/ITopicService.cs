using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface ITopicService
    {
        IEnumerable<TopicBusiness> GetTopics();

        TopicBusiness GetCeratainTopic(string topicId);
        void Create(string name);
        void Delete(string id, string webrootPath);
        void Update(TopicBusiness topic);
        void Dispose();
    }
}
