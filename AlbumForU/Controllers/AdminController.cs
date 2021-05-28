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
    [Authorize(Roles = "admin, manager")]
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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AppointSmn()
        {
            List<AppUser> appUsers = (from user in appDataContext.Users
                                      select user).ToList();
            return View(appUsers);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AppointCertainUser(UserRoleRelation userRole)
        {
            if(ModelState.IsValid)
            {
                appDataContext.UserRoles.Add(new IdentityUserRole<string> { UserId = userRole.user.Id, RoleId = userRole.chosenRoleId });
                appDataContext.SaveChanges();
                TempData["Success"] = $"User {userRole.user.Nickname} was appointes to role with id {userRole.chosenRoleId}";
                return RedirectToAction("AdminIndex");
            }

            return View(userRole);
        }

        [HttpGet]
        public IActionResult AddNewTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewTopic(string topicName)
        {
            if(ModelState.IsValid)
            {
                List<string> names = (from topic in appDataContext.Topics
                                      select topic.Name).ToList();
                if(names.Count()>0)
                {
                    foreach (var name in names)
                    {
                        if (name == topicName)
                        {
                            ModelState.AddModelError("", "This topic is alredy exist!");
                            return View();
                        }
                    }
                }
                appDataContext.Topics.Add(new Topic() { Name = topicName });
                appDataContext.SaveChanges();
                TempData["Success"] = $"Topic {topicName} was successfully added!";
                return RedirectToAction("AdminIndex");
            }
            ModelState.AddModelError("", "Fill in all fields!");
            return View();
        }
    }
}
