using AlbumForU.Models;
using AlbumForU.ViewModels;
using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlbumForU.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IPictureService _pictureService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly ITopicService _topicService;
        private readonly IRoleService _roleService;
        private readonly IAppUserService _appUserService;
        IWebHostEnvironment _appEnvironment;

        public AccountController(IWebHostEnvironment appEnv, IPictureService pics, ICommentService cms, ILikeService lks, ITopicService tcs, IRoleService rls, IAppUserService usrs)
        {
            _pictureService = pics;
            _commentService = cms;
            _likeService = lks;
            _topicService = tcs;
            _roleService = rls;
            _appUserService = usrs;
            _appEnvironment = appEnv;

        }
        [Route("~/Account/ManagePictures/{userId}")]
        public ActionResult ReturnUserPhotos(string userId)
        {

            HomeViewModel viewModel = new HomeViewModel();

            List<ThumbnailBusiness> thumbnailBusinesses = _pictureService.GetUserThumbs(userId).ToList();
            var mapperThumbs = new MapperConfiguration(cfg => cfg.CreateMap<ThumbnailBusiness, Thumbnail>()).CreateMapper();
            List<Thumbnail> thumbnails = mapperThumbs.Map<IEnumerable<ThumbnailBusiness>, List<Thumbnail>>(thumbnailBusinesses);

            int volume = thumbnails.Count() / 3;
            int remainder = thumbnails.Count() % 3;


            viewModel.ThumbsFirstColumn = (from thumb in thumbnails
                                           .ToList().Where((s, i) => i < volume)
                                           select thumb).ToList();


            viewModel.ThumbsSecondColumn = (from thumb in thumbnails
                                            .ToList().Where((s, i) => i >= volume && i < volume * 2)
                                            select thumb).ToList();

            viewModel.ThumbsThirdColumn = (from thumb in thumbnails
                                           .ToList().Where((s, i) => i >= volume * 2 && i < volume * 3 + remainder)
                                           select thumb).ToList();

            return View(viewModel);
        }
        [Route("~/Account/CertainPicture/{originalId}")]
        [HttpGet]
        public ActionResult ReturnCeratainUserPhoto(string originalId)
        {

            PictureBusiness pictureBusiness = _pictureService.GetCeratainPicture(originalId);
            var mapperPictures = new MapperConfiguration(cfg => cfg.CreateMap<PictureBusiness, Picture>()).CreateMapper();
            Picture picture = mapperPictures.Map<PictureBusiness, Picture>(pictureBusiness);

            return View(picture);
        }
        [Route("~/Account/CertainPicture/{originalId}")]
        [HttpPost]
        public ActionResult ReturnCeratainUserPhoto(string Id,string Name)
        {
            if(Id!=null && Name!=null)
            {
                try
                {
                    PictureBusiness updatetedPic = _pictureService.GetCeratainPicture(Id);
                    updatetedPic.Name = Name;
                    _pictureService.Update(updatetedPic);

                    TempData["Success"] = $"Picture was successfully edited!";
                    return Redirect("~/Account/CertainPicture/" + Id);
                }
                catch (Exception ex)
                {
                    TempData["Failure"] = $"{ex.Message}";
                }

            }
            else
            {
                TempData["Failure"] = $"Fill in fields!";
            }

            PictureBusiness pictureBusiness = _pictureService.GetCeratainPicture(Id);
            var mapperPictures = new MapperConfiguration(cfg => cfg.CreateMap<PictureBusiness, Picture>()).CreateMapper();
            Picture picture = mapperPictures.Map<PictureBusiness, Picture>(pictureBusiness);

            return View(picture);
        }
        
        [Route("~/Account/CertainPicture/Delete/{Id}")]
        public ActionResult DeleteOneOfUserPicture(string Id)
        {

            _pictureService.Delete(Id, _appEnvironment.WebRootPath);
            TempData["Success"] = $"Picture was successfully deleted!";

            return Redirect("~/Account/ManagePictures/"+ this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [Route("~/Account/ManageComments/{userId}")]
        public ActionResult ReturnUserComments(string userId)
        {
            List<CommentBusiness> commentBusinessList = _commentService.FindUsersComments(userId);
            var mapperComments = new MapperConfiguration(cfg => cfg.CreateMap<CommentBusiness, Comment>()).CreateMapper();
            List<Comment> Comments = mapperComments.Map<List<CommentBusiness>, List<Comment>>(commentBusinessList);

            return View(Comments);
        }
        [Route("~/Account/ManageComments/Delete/{commentId}")]
        public ActionResult DeleteUserComments(string commentId)
        {
            if(commentId!=null)
            {
                _commentService.DeleteComment(commentId);
                TempData["Success"] = $"Comment was successfully deleted!";
            }
            else
            {
                ModelState.AddModelError("", "Can`t delete this comment at the moment");
            }
            return Redirect("~/Account/ManageComments/" + this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }


    }
}
