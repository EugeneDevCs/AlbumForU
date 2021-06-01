using AlbumForU.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlbumForU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.BusinessModels;
using AutoMapper;

namespace AlbumForU.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class AdminController : Controller
    {
        private readonly IPictureService _pictureService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly ITopicService _topicService;
        private readonly IRoleService _roleService;
        private readonly IAppUserService _appUserService;

        public AdminController(IPictureService pics, ICommentService cms, ILikeService lks, ITopicService tcs, IRoleService rls, IAppUserService usrs)
        {
            _pictureService = pics;
            _commentService = cms;
            _likeService = lks;
            _topicService = tcs;
            _roleService = rls;
            _appUserService = usrs;

        }
        [Route("~/Admin/AdminIndex")]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [Route("~/Admin/AppointSmn")]
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AppointSmn()
        {
            List<AppUserBusiness> appUserBusinesses = _appUserService.GetAppUsers().ToList();
            var mapperUsers = new MapperConfiguration(cfg => cfg.CreateMap<AppUserBusiness, AppUser>()).CreateMapper();
            List<AppUser> appUsers = mapperUsers.Map<IEnumerable<AppUserBusiness>, List<AppUser>>(appUserBusinesses);
            return View(appUsers);
        }

        [Route("~/Admin/AppointCertainUser/{id}")]
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AppointCertainUser(string id)
        {
            UserRoleRelation userRole = new UserRoleRelation();

            AppUserBusiness appUserBusiness = _appUserService.GetUser(id);
            var mapperUser = new MapperConfiguration(cfg => cfg.CreateMap<AppUserBusiness, AppUser>()).CreateMapper();
            userRole.user = mapperUser.Map<AppUserBusiness, AppUser>(appUserBusiness);

            userRole.roles = _roleService.GetRoles().ToList();
            
            return View(userRole);
        }

        [Route("~/Admin/AppointCertainUser/{id}")]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AppointCertainUser(UserRoleRelation userRole)
        {
            if(ModelState.IsValid)
            {
                _roleService.AppointSomeone(userRole.chosenRoleId, userRole.user.Id);
                TempData["Success"] = $"User {userRole.user.Nickname} was appointes to role with id {userRole.chosenRoleId}";
                return RedirectToAction("AdminIndex");
            }
            ModelState.AddModelError("", "Fill in all fields!");
            return View(userRole);
        }

        [Route("~/Admin/MangeTopics")]
        [HttpGet]
        public IActionResult MangeTopics()
        {
            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
            var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            List<Topic>topics = mapperTopic.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses).ToList();
            
            return View(topics);
        }

        [Route("~/Admin/MangeTopics/Delete{id}")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if(id!=null)
            {
                _topicService.Delete(id);
            }
            else
            {
                ModelState.AddModelError("", "There is a problem!");
            }
                       
            return Redirect("~/Admin/MangeTopics");
        }

        [Route("~/Admin/MangeTopics/Edit/{id}")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            TopicBusiness topicBusiness = _topicService.GetCeratainTopic(id);
            var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            Topic topic = mapperTopic.Map<TopicBusiness, Topic>(topicBusiness);

            return View(topic);
        }
        
        [Route("~/Admin/MangeTopics/Edit{id}")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            TopicBusiness topicBusiness = _topicService.GetCeratainTopic(id);
            var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            Topic topic = mapperTopic.Map<TopicBusiness, Topic>(topicBusiness);

            return View(topic);
        }

        [Route("~/Admin/MangeTopics")]
        [HttpPost]
        public IActionResult AddNewTopic(string topicName)
        {
            if(ModelState.IsValid)
            {
                List<string> names = (from topic in _topicService.GetTopics().ToList()
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

                _topicService.Create(topicName);
                TempData["Success"] = $"Topic {topicName} was successfully added!";
                return RedirectToAction("AddNewTopic");
            }
            ModelState.AddModelError("", "Fill in all fields!");
            return View();
        }

        //[Route("~/Admin/MangePictures")]
        //[HttpGet]
        //public IActionResult MangePictures()
        //{
        //    List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
        //    var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
        //    List<Topic>topics = mapperTopic.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses).ToList();
            
        //    return View(topics);
        //}

        //[Route("~/Admin/MangePictures/{id}")]
        //[HttpPost]
        //public IActionResult MangePictures(string id)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        List<string> names = (from topic in _topicService.GetTopics().ToList()
        //                              select topic.Name).ToList();
        //        if(names.Count()>0)
        //        {
        //            foreach (var name in names)
        //            {
        //                if (name == topicName)
        //                {
        //                    ModelState.AddModelError("", "This topic is alredy exist!");
        //                    return View();
        //                }
        //            }
        //        }

        //        _topicService.Create(topicName);
        //        TempData["Success"] = $"Topic {topicName} was successfully added!";
        //        return RedirectToAction("AddNewTopic");
        //    }
        //    ModelState.AddModelError("", "Fill in all fields!");
        //    return View();
        //}
    }
}
