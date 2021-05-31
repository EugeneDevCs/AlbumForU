﻿using AlbumForU.ViewModels;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using AlbumForU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.BusinessModels;

namespace AlbumForU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPictureService _pictureService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly ITopicService _topicService;


        public HomeController(ILogger<HomeController> logger, IPictureService pics, ICommentService cms, ILikeService lks, ITopicService tcs)
        {
            _logger = logger;
            _pictureService = pics;
            _commentService = cms;
            _likeService = lks;
            _topicService = tcs;

        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();


            List<ThumbnailBusiness>thumbnailBusinesses = _pictureService.GetThumbs().ToList();
            var mapperThumbs = new MapperConfiguration(cfg => cfg.CreateMap<ThumbnailBusiness, Thumbnail>()).CreateMapper();
            List<Thumbnail> thumbnails = mapperThumbs.Map<IEnumerable<ThumbnailBusiness>, List<Thumbnail>>(thumbnailBusinesses);

            List<TopicBusiness>topicBusinesses = _topicService.GetTopics().ToList();
            var mapperTopics = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
            viewModel.Topics = mapperTopics.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses);

                      

            viewModel.ThumbsFirstColumn = (from thumb in thumbnails
                                           .ToList().Where((s, i) => i < 3)
                                           select thumb).ToList();


            viewModel.ThumbsSecondColumn = (from thumb in thumbnails
                                            .ToList().Where((s, i) => i >= 3 && i < 6)
                                            select thumb).ToList();

            viewModel.ThumbsThirdColumn = (from thumb in thumbnails
                                           .ToList().Where((s, i) => i >= 6 && i < 9)
                                           select thumb).ToList();
            if (thumbnails.Count() > 9)
            {
                ViewBag.PrevPage = 0;
                ViewBag.NextPageNum = 2;
            }
            return View(viewModel);
        }

        //[Route("Home/Index/{page}")]
        //public IActionResult Index(int page)
        //{
        //    if(page > 1)
        //    {
        //        int indexFirst = 9 * (page - 1);
        //        int indexSecond = indexFirst + 3;
        //        int indexThird = indexSecond + 3;

        //        HomeViewModel viewModel = new HomeViewModel();
        //        viewModel.Topics = (from topic in appData.Topics
        //                            select topic).ToList();

        //        List<Thumbnail> thumbnails = (from thum in appData.Thumbs
        //                                      select thum).ToList();

        //        viewModel.ThumbsFirstColumn = (from thumb in thumbnails
        //                                       .ToList().Where((s, i) => i >= indexFirst && i < indexSecond)
        //                                       select thumb).ToList();

        //        viewModel.ThumbsSecondColumn = (from thumb in thumbnails
        //                                        .ToList().Where((s, i) => i >= indexSecond && i < indexThird)
        //                                        select thumb).ToList();

        //        viewModel.ThumbsThirdColumn = (from thumb in thumbnails
        //                                       .ToList().Where((s, i) => i >= indexThird && i < indexThird + 3)
        //                                       select thumb).ToList();

        //        ViewBag.PrevPage = page - 1;
        //        ViewBag.NextPageNum = page + 1;
        //        return View(viewModel);
        //    }
        //    else
        //    {
        //        return Redirect("~/Home/Index");
        //    }
            
        //}

        //[Route("Home/Index/{topicId}/{page}")]
        //public IActionResult Index(string topicId, int page)
        //{
        //    int indexFirst;
        //    int indexSecond;
        //    int indexThird;
        //    if (page==0)
        //    {
        //        indexFirst = 0;
        //        indexSecond = indexFirst + 3;
        //        indexThird = indexSecond + 3;
        //    }
        //    else
        //    {
        //         indexFirst = 9 * (page - 1);
        //         indexSecond = indexFirst + 3;
        //         indexThird = indexSecond + 3;
        //    }
            

        //    HomeViewModel viewModel = new HomeViewModel();
        //    viewModel.Topics = (from topic in appData.Topics
        //                        select topic).ToList();

        //    List<Thumbnail> thumbnails = (from thum in appData.Thumbs
        //                                  where thum.TopicId == topicId
        //                                  select thum).ToList();

        //    viewModel.ThumbsFirstColumn = (from thumb in thumbnails
        //                                   .ToList().Where((s, i) => i >= indexFirst && i < indexSecond)
        //                                   select thumb).ToList();

        //    viewModel.ThumbsSecondColumn = (from thumb in thumbnails
        //                                    .ToList().Where((s, i) => i >= indexSecond && i < indexThird)
        //                                    select thumb).ToList();

        //    viewModel.ThumbsThirdColumn = (from thumb in thumbnails
        //                                   .ToList().Where((s, i) => i >= indexThird && i < indexThird + 3)
        //                                   select thumb).ToList();
        //    if (page == 1 && thumbnails.Count() > 9)
        //    {
        //        ViewBag.PrevPage = 0;
        //        ViewBag.NextPageNum = 2;
        //    }
        //    else if(page>1)
        //    {
        //        ViewBag.PrevPage = page - 1;
        //        ViewBag.NextPageNum = page + 1;
        //    }
            
        //    return View(viewModel);

        //}

        //[HttpPost]
        //public IActionResult Search(string request_text)
        //{
        //    HomeViewModel viewModel = new HomeViewModel();
        //    viewModel.Topics = (from topic in appData.Topics
        //                        select topic).ToList();
        //    List<Thumbnail> thumbnails = (from thum in appData.Thumbs
        //                                  join orig in appData.Pictures on thum.OriginalId equals orig.Id
        //                                  where orig.Name.Contains(request_text) == true
        //                                  select thum).ToList();

        //    viewModel.ThumbsFirstColumn = (from thumb in thumbnails
        //                                   .ToList().Where((s, i) => i < 3)
        //                                   select thumb).ToList();


        //    viewModel.ThumbsSecondColumn = (from thumb in thumbnails
        //                                    .ToList().Where((s, i) => i >= 3 && i < 6)
        //                                    select thumb).ToList();

        //    viewModel.ThumbsThirdColumn = (from thumb in thumbnails
        //                                   .ToList().Where((s, i) => i >= 6 && i < 9)
        //                                   select thumb).ToList();
        //    if (thumbnails.Count() > 9)
        //    {
        //        ViewBag.PrevPage = 0;
        //        ViewBag.NextPageNum = 2;
        //    }
        //    return View(viewModel);
        //}
        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
