﻿using AlbumForU.ViewModels;
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
        IWebHostEnvironment _appEnvironment;
        public PictureController(IWebHostEnvironment appEnvironment, IPictureService pics, ICommentService cms, ILikeService lks, ITopicService tcs)
        {
            _pictureService = pics;
            _commentService = cms;
            _likeService = lks;
            _topicService = tcs;
            _appEnvironment = appEnvironment;
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
            return View();


        }

            //    viewModel.Topics = (from topic in appData.Topics
            //                        select topic).ToList();
            //    ModelState.AddModelError("", "Fill in all fields!");
            //    return View(viewModel);
            //}


            //[Route("Picture/CertainPicture/{originalId}")]
            //public IActionResult CertainPicture(string originalId)
            //{
            //    PictureViewModel picture = new PictureViewModel();
            //    string id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //    picture.Picture = (from pic in appData.Pictures
            //                       where pic.Id == originalId
            //                       select pic).FirstOrDefault();
            //    picture.Comments = (from cmt in appData.Comments
            //                     where cmt.PictureId == originalId
            //                     select cmt).ToList();
            //    picture.Like = (from like in appData.Likes
            //                     where like.PictureId == originalId
            //                     select like).FirstOrDefault();
            //    picture.appUser = (from user in appData.Users
            //                       where user.Id == picture.Picture.UserId
            //                       select user).FirstOrDefault();

            //    TempData["originalId"] = originalId;
            //    return View(picture);
            //}

            //[HttpPost]
            //public IActionResult AddAComment(string commentBody)
            //{
            //    if(ModelState.IsValid && commentBody.Length>0)
            //    {
            //        Comment comment = new Comment()
            //        {
            //            dateTime = DateTime.Today,
            //            UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
            //            UserNick = (from user in appData.Users
            //                        where user.Id == this.User.FindFirst(ClaimTypes.NameIdentifier).Value
            //                        select user.Nickname).FirstOrDefault().ToString(),
            //            TextBody=commentBody,
            //            PictureId= TempData["originalId"].ToString()
            //        };
            //        appData.Comments.Add(comment);
            //        appData.SaveChanges();

            //    }            
            //    return Redirect("CertainPicture/" + TempData["originalId"]);
            //}
            //public IActionResult LikeDislike(int like, string pictId)
            //{
            //    if(like == 1)
            //    {
            //        appData.Likes.Add(new Like { DateTime = DateTime.Today, PictureId = pictId, UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value });
            //        appData.SaveChanges();
            //    }
            //    else
            //    {
            //        Like removeLike = (from lk in appData.Likes
            //                            where lk.PictureId == pictId && lk.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value
            //                            select lk).FirstOrDefault();
            //        appData.Likes.Remove(removeLike);
            //        appData.SaveChanges();
            //    }
            //    return Redirect("CertainPicture/" + pictId);
            //}
    }
}

