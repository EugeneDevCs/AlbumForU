using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Services
{
    public class AppUserService : IAppUserService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get; set; }
        public AppUserService(IDbAccess db)
        {
            dbAccess = db;
        }
        //here I use automapper to obtain objects from Server layer 
        //and map them to BL models
        public IEnumerable<AppUserBusiness> GetAppUsers()
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<AppUser, AppUserBusiness>()).CreateMapper();
            List<AppUserBusiness> appUsers = mappedData.Map<IEnumerable<AppUser>, List<AppUserBusiness>>(dbAccess.Users.GetAll());
            return appUsers;
        }

        public AppUserBusiness GetUser(string id)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<AppUser, AppUserBusiness>()).CreateMapper();
            AppUserBusiness appUser= mappedData.Map<AppUser, AppUserBusiness>(dbAccess.Users.Get(id));
            return appUser;
        }

    }
}
