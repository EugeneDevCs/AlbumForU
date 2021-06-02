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
    public class AppUserRepository : IRepository<AppUser>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public AppUserRepository(AppDataContext data)
        {
            _appData = data;
        }

        public AppUser Create(AppUser item)
        {
            return _appData.Users.Add(item).Entity;
        }

        public void Delete(string id)
        {
            AppUser user = _appData.Users.Find(id);
            if (user != null)
            {
                _appData.Users.Remove(user);
            }
        }

        public IEnumerable<AppUser> Find(Func<AppUser, bool> predicate)
        {
            return _appData.Users.Where(predicate);
        }

        public AppUser Get(string id)
        {
            return _appData.Users.Find(id);
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _appData.Users;
        }

        public void Update(AppUser item)
        {
            //here I create new instance of user 
            //to avoid tracking
            AppUser appUser = _appData.Users.Find(item.Id);

            appUser.Lastname = item.Lastname;
            appUser.Firstname = item.Firstname;
            appUser.Nickname = item.Nickname;
            appUser.Email = item.Email;

            _appData.Users.Update(appUser);
        }
    }
}
