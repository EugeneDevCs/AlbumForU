using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.AdditionalFunctional
{
    public class ErrorWhilwSavingToDBException:Exception
    {
        public ErrorWhilwSavingToDBException(string msg)
            : base(msg)
        {

        }
    }
}
