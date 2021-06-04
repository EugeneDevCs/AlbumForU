using Microsoft.AspNetCore.Identity;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.Interfaces
{
    public interface IUserRoleRepository
    {
        void Appoint(string roleId,string userId);
        void Disappoint(string roleId, string userId);
        List<IdentityUserRole<string>> GetUserRoles();

    }
}
