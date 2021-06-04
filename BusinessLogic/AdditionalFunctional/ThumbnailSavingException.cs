using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AdditionalFunctional
{
    public class ThumbnailSavingException :Exception
    {
        public ThumbnailSavingException(string msg)
            : base(msg)
        {

        }
    }
}
