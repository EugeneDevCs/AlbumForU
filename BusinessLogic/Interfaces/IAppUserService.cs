using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface IAppUserService
    {
        IEnumerable<AppUserBusiness> GetAppUsers();
        AppUserBusiness GetUser(string id);
        void Dispose();

    }
}
