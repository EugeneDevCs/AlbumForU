using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;
using ServerLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get; set; }
        public RoleService(IDbAccess db)
        {
            dbAccess = db;
        }
        //here I DON`T use automapper to obtain objects from Server layer 
        //because I use the common role for every layer
        //which is IdentityRole
        public IdentityRole AddRole(string name)
        {
            if (name != null)
            {
                IdentityRole role = dbAccess.Roles.Create(new IdentityRole { Name = name });
                dbAccess.Save();
                return role;
            }
            else
            {
                throw new ArgumentNullException("Not enough information");
            }

            
        }

        public IdentityRole GetRole(string id)
        {
            return dbAccess.Roles.Get(id);
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return dbAccess.Roles.GetAll();
        }

        public void AppointSomeone(string roleId, string userId)
        {
            if(roleId !=null && userId !=null)
            {
                dbAccess.UserRoles.Appoint(roleId, userId);
                dbAccess.Save();
            }
            else
            {
                throw new ArgumentNullException("Not enough information");
            }
        }

        public void DisappointSomeone(string roleId, string userId)
        {
            if (roleId != null && userId != null)
            {
                dbAccess.UserRoles.Disappoint(roleId, userId);
                dbAccess.Save();
            }
            else
            {
                throw new ArgumentNullException("Not enough information");
            }
        }
    }
}
