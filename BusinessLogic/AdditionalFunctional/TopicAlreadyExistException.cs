using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AdditionalFunctional
{
    public class TopicAlreadyExistException:ArgumentException
    {
        public TopicAlreadyExistException(string msg)
            :base (msg)
        {

        }
    }
}
