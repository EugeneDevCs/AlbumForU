using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLayer.Repositories
{
    public class RoleRepository : IRepository<IdentityRole>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public RoleRepository(AppDataContext data)
        {
            _appData = data;
        }


        public IdentityRole Create(IdentityRole item)
        {
            return _appData.Roles.Add(item).Entity;
        }

        public void Delete(string id)
        {
            IdentityRole role = _appData.Roles.Find(id);
            if (role != null)
            {
                _appData.Roles.Remove(role);
            }
        }

        public IEnumerable<IdentityRole> Find(Func<IdentityRole, bool> predicate)
        {
            return _appData.Roles.Where(predicate);
        }

        public IdentityRole Get(string id)
        {
            return _appData.Roles.Find(id);
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return _appData.Roles;
        }

        public void Update(IdentityRole item)
        {
            //here I create new instance of role 
            //to avoid tracking 
            IdentityRole role = _appData.Roles.Find(item.Id);

            role.Name = item.Name;

            _appData.Roles.Update(item);
        }
    }
}
