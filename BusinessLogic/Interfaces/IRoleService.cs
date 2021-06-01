using BusinessLogic.BusinessModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<IdentityRole> GetRoles();
        IdentityRole GetRole(string id);
        IdentityRole AddRole(string name);
        void AppointSomeone(string roleId, string userId);
        void DisappointSomeone(string roleId, string userId);
    }
}
