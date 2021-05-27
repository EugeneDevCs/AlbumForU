using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AddANewRole()
        {
            List<IdentityRole> roles = (from role in appDataContext.Roles
                                        select role).ToList();
            return View(roles);
        }
        public IActionResult AppointSmn()
        {
            List<IdentityRole> roles = (from role in appDataContext.Roles
                                        select role).ToList();
            List<AppUser> appUsers = (from user in appDataContext.Users
                                      select user).ToList();
            return View(roles,appUsers);
        }
    }
}
