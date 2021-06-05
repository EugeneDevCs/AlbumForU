using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<IdentityUserRole<string>> GetUserRoles()
        {
            List<IdentityUserRole<string>> userRoles = _appData.UserRoles.ToList();
            return userRoles;
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
            IdentityUserRole<string> DelRole = (from ur in _appData.UserRoles
                                                where ur.RoleId == roleId && ur.UserId == userId
                                                select ur).FirstOrDefault();
            _appData.UserRoles.Remove(DelRole);
        }
    }
}
