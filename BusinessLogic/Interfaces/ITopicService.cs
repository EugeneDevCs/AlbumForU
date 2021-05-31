﻿using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface ITopicService
    {
        IEnumerable<TopicBusiness> GetTopics();
        
    }
}
