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
using Microsoft.AspNetCore.Hosting;

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
        IWebHostEnvironment _appEnvironment;

        public AdminController(IWebHostEnvironment appEnv, IPictureService pics, ICommentService cms, ILikeService lks, ITopicService tcs, IRoleService rls, IAppUserService usrs)
        {
            _pictureService = pics;
            _commentService = cms;
            _likeService = lks;
            _topicService = tcs;
            _roleService = rls;
            _appUserService = usrs;
            _appEnvironment = appEnv;

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

        [Route("~/Admin/ManageTopics")]
        [HttpGet]
        public IActionResult ManageTopics()
        {
            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
            var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            List<Topic>topics = mapperTopic.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses).ToList();
            
            return View(topics);
        }
        
        [Route("~/Admin/ManageTopics")]
        [HttpPost]
        public IActionResult ManageTopics(string topicName)
        {
            if (ModelState.IsValid)
            {
                List<string> names = (from topic in _topicService.GetTopics().ToList()
                                      select topic.Name).ToList();
                if (names.Count() > 0)
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
                return Redirect("~/Admin/ManageTopics");
            }
            ModelState.AddModelError("", "Fill in all fields!");
            return View();
        }

        [Route("~/Admin/ManageTopics/Delete/{id}")]
        [HttpGet]
        public IActionResult DeleteTopic(string id)
        {
            
            if(id!=null)
            {
                _topicService.Delete(id, _appEnvironment.WebRootPath);
            }
            else
            {
                ModelState.AddModelError("", "There is a problem!");
            }
                       
            return Redirect("~/Admin/ManageTopics");
        }

        [Route("~/Admin/ManageTopics/Edit/{id}")]
        [HttpGet]
        public IActionResult EditTopic(string id)
        {
            TopicBusiness topicBusiness = _topicService.GetCeratainTopic(id);
            var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            Topic topic = mapperTopic.Map<TopicBusiness, Topic>(topicBusiness);

            return View(topic);
        }
        
        [Route("~/Admin/ManageTopics/Edit/{id}")]
        [HttpPost]
        public IActionResult EditTopic(Topic topic)
        {
            if(ModelState.IsValid)
            {
                TopicBusiness topicBusiness = _topicService.GetCeratainTopic(topic.Id);
                topicBusiness.Name = topic.Name;
                _topicService.Update(topicBusiness);
                return Redirect("~/Admin/ManageTopics");
            }
            ModelState.AddModelError("", "Something wrong!");
            return View(topic);
        }

        [Route("~/Admin/DeletePictures/{id}")]
        [HttpGet]
        public IActionResult DeletePictures(string id)
        {
            PictureBusiness pictureBusiness = _pictureService.GetCeratainPicture(id);
            var mapperPictures = new MapperConfiguration(cfg => cfg.CreateMap<PictureBusiness, Picture>()).CreateMapper();
            Picture picture = mapperPictures.Map<PictureBusiness, Picture>(pictureBusiness);

            return View(picture);
        }

        [Route("~/Admin/DeletePictures/Agree/{id}")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            _pictureService.Delete(id, _appEnvironment.WebRootPath);
            TempData["Success"] = $"Picture was successfully deleted!";
            return Redirect("~/");
        }
        
        [Route("~/Admin/DeleteComment")]
        [HttpGet]
        public IActionResult DeleteComment(string id,string pictId)
        {
            _commentService.DeleteComment(id);
            TempData["Success"] = $"Comment was successfully deleted!";
            return Redirect("~/Picture/CertainPicture/"+ pictId);
        }
    }
}
