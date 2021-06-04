using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AdditionalFunctional
{
    public class PictureSavingException :Exception
    {
        public PictureSavingException(string msg)
            :base(msg)
        {

        }
    }
}
