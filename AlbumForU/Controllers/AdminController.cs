using AlbumForU.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServerLayer.DataObtaining;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumForU.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDataContext appDataContext;
        public AdminController(AppDataContext dataContext)
        {
            appDataContext = dataContext;
        }
        public IActionResult AdminIndex()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AppointSmn()
        {
            List<AppUser> appUsers = (from user in appDataContext.Users
                                      select user).ToList();
            return View(appUsers);
        }

        [HttpGet]
        public IActionResult AppointCertainUser(string id)
        {
            UserRoleRelation userRole = new UserRoleRelation();
            userRole.user = (from user in appDataContext.Users
                            where user.Id == id
                            select user).FirstOrDefault();
            userRole.roles = (from role in appDataContext.Roles
                                        select role).ToList();
           
            return View(userRole);
        }

        [HttpPost]
        public IActionResult AppointCertainUser(UserRoleRelation userRole)
        {
            if(ModelState.IsValid)
            {
                appDataContext.UserRoles.Add(new IdentityUserRole<string> { UserId = userRole.user.Id, RoleId = userRole.chosenRoleId });
                appDataContext.SaveChanges();
            }
            return View(userRole);
        }
    }
}
