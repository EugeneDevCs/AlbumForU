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
using BusinessLogic.AdditionalFunctional;

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

            if(_roleService.GetUserRole(id)!=null)
            {
                userRole.currentRoleId = _roleService.GetUserRole(id).Id;
                userRole.roles.RemoveAll(role => role.Id == userRole.currentRoleId);                
                TempData["CurrentRole"] = _roleService.GetUserRole(id).Name;
            }
            

            return View(userRole);
        }

        [Route("~/Admin/AppointCertainUser/{id}")]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AppointCertainUser(UserRoleRelation userRole)
        {
            if(ModelState.IsValid && userRole.chosenRoleId!=null)
            {
                try
                {
                    _roleService.AppointSomeone(userRole.chosenRoleId, userRole.user.Id);
                    TempData["Success"] = $"User {userRole.user.Nickname} was appointed to role with id {userRole.chosenRoleId}";
                    return RedirectToAction("AdminIndex");
                }
                catch (Exception ex)
                {
                    TempData["Failure"] = $"{ex.Message}";
                }
                
            }
            else
            {
                TempData["Failure"] = $"Fill in all fields!";
            }
            return Redirect("~/Admin/AppointCertainUser/"+userRole.user.Id);
        }

        [Route("~/Admin/DisappointCertainUser/{roleId}/{userId}")]
        [Authorize(Roles = "admin")]
        public IActionResult DisappointCertainUser(string roleId,string userId)
        {
            if(ModelState.IsValid && userId!=null)
            {
                try
                {
                    _roleService.DisappointSomeone(roleId,userId);
                    TempData["Success"] = $"User was successfully disappointed";
                }
                catch (Exception)
                {
                    throw;
                }
                
                return RedirectToAction("AdminIndex");
            }
            ModelState.AddModelError("", "Fill in all fields!");
            return RedirectToAction("AdminIndex");
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
            if (ModelState.IsValid && topicName!= null)
            {
                try
                {
                    _topicService.Create(topicName);
                    TempData["Success"] = $"Topic {topicName} was successfully added!";
                }
                catch(TopicAlreadyExistException ex)
                {
                    ModelState.AddModelError("", ex.Message);             
                }
            }
            else
            {
                ModelState.AddModelError("", "Fill in all fields!");
            }
            var mapperTopic = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();            
            List<Topic> topics = mapperTopic.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses).ToList();
            return View(topics);
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
            if(ModelState.IsValid && topic.Name!=null)
            {
                try
                {
                    TopicBusiness topicBusiness = _topicService.GetCeratainTopic(topic.Id);
                    topicBusiness.Name = topic.Name;
                    _topicService.Update(topicBusiness);
                }
                catch (TopicAlreadyExistException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {

                ModelState.AddModelError("", "Fill in only correct values!");
                return View(topic);
            }
            return Redirect("~/Admin/ManageTopics");
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
