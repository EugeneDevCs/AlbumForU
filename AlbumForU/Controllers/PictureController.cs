using AlbumForU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using BusinessLogic.Interfaces;
using BusinessLogic.BusinessModels;
using AutoMapper;
using AlbumForU.Models;

namespace AlbumForU.Controllers
{
    [Authorize]
    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly ITopicService _topicService;
        private readonly IAppUserService _appUserService;
        IWebHostEnvironment _appEnvironment;
        public PictureController(IWebHostEnvironment appEnvironment, IPictureService pics, ICommentService cms, ILikeService lks, ITopicService tcs, IAppUserService usrs)
        {
            _pictureService = pics;
            _commentService = cms;
            _likeService = lks;
            _topicService = tcs;
            _appEnvironment = appEnvironment;
            _appUserService = usrs;
        }

        [HttpGet]
        [Route("/Picture/AddAPicture")]
        public IActionResult AddAPicture()
        {
            PictureCreationViewModel viewModel = new PictureCreationViewModel();

            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
            var mapperTopics = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            viewModel.Topics = mapperTopics.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses);

            return View(viewModel);
        }

        [HttpPost]
        [Route("/Picture/AddAPicture")]
        public IActionResult AddAPicture(PictureCreationViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.Picture != null)
            {
                string currentUserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                _pictureService.AddPicture(
                    viewModel.PictureName,
                    viewModel.TopicId,
                    viewModel.Picture,
                    _appEnvironment.WebRootPath,
                    currentUserID);
                TempData["Success"] = $"Picture was successfully uploaded!";
                return Redirect("/Home/Index");
            }
            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
            var mapperTopics = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            viewModel.Topics = mapperTopics.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses);
            ModelState.AddModelError("", "Fill in all fields!");
            return View(viewModel);
        }

        [Route("Picture/CertainPicture/{originalId}")]
        public IActionResult CertainPicture(string originalId)
        {
            PictureViewModel picture = new PictureViewModel();
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //obtaining the picture
            PictureBusiness pictureBusiness = _pictureService.GetCeratainPicture(originalId);
            var mapperPictures = new MapperConfiguration(cfg => cfg.CreateMap<PictureBusiness, Picture>()).CreateMapper();
            picture.Picture = mapperPictures.Map<PictureBusiness, Picture>(pictureBusiness);

            //obtaining the comments
            List<CommentBusiness> commentBusinesses = _commentService.GetComments(originalId).ToList();
            var mapperComments = new MapperConfiguration(cfg => cfg.CreateMap<CommentBusiness, Comment>()).CreateMapper();
            picture.Comments = mapperComments.Map<IEnumerable<CommentBusiness>, List<Comment>>(commentBusinesses);

            //obtaining the user
            AppUserBusiness appUserBusiness = _appUserService.GetUser(picture.Picture.UserId);
            var mapperUser = new MapperConfiguration(cfg => cfg.CreateMap<AppUserBusiness, AppUser>()).CreateMapper();
            picture.appUser = mapperUser.Map<AppUserBusiness, AppUser>(appUserBusiness);

            //Quantity of Likes
            picture.QuantityLikes = _likeService.CountLikes(originalId);

            //Is liked?
            picture.IsLiked = _likeService.IsLiked(id, originalId);

            TempData["originalId"] = originalId;
            return View(picture);
        }

        [HttpPost]
        [Route("Picture/AddAComment")]
        public IActionResult AddAComment(string commentBody)
        {
            if (ModelState.IsValid && commentBody.Length > 0)
            {
                _commentService.AddAComment(
                    DateTime.Today,
                    this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    commentBody,
                    TempData["originalId"].ToString());               

            }
            return Redirect("CertainPicture/" + TempData["originalId"]);
        }

        [Route("Picture/LikeDislike/{pictId}")]
        public IActionResult LikeDislike(string pictId)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _likeService.ToLikeDisLike(userId, pictId);
            return Redirect("~/Picture/CertainPicture/" + pictId);
        }
        
        [Route("Picture/Delete/{pictId}")]
        public IActionResult Delete(string pictId)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _likeService.ToLikeDisLike(userId, pictId);
            return Redirect("~/Picture/CertainPicture/" + pictId);
        }


    }
}

