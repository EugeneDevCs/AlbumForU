using Microsoft.AspNetCore.Identity;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public UserRoleRepository(AppDataContext data)
        {
            _appData = data;
        }

        public void Appoint(string roleId, string userId)
        {
            _appData.UserRoles.Add(new IdentityUserRole<string> {
                RoleId=roleId,
                UserId=userId
            });
        }

        public void Disappoint(string roleId, string userId)
        {
            _appData.UserRoles.Remove(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = userId
            });
        }
    }
}
