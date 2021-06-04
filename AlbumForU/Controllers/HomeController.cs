using AlbumForU.ViewModels;
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
        [Route("")]
        [Route("Home/Index/{page?}")]
        public IActionResult Index(int page = 1)
        {
            HomeViewModel viewModel = new HomeViewModel();

            List<ThumbnailBusiness> thumbnailBusinesses = _pictureService.GetThumbs(page).ToList();
            var mapperThumbs = new MapperConfiguration(cfg => cfg.CreateMap<ThumbnailBusiness, Thumbnail>()).CreateMapper();
            List<Thumbnail> thumbnails = mapperThumbs.Map<IEnumerable<ThumbnailBusiness>, List<Thumbnail>>(thumbnailBusinesses);

            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
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
            if (thumbnails.Count() > 9 && page == 1)
            {
                ViewBag.NextPageNum = 2;
            }
            else if (thumbnails.Count() > 9)
            {
                ViewBag.PrevPage = page - 1;
                ViewBag.NextPageNum = page + 1;
            }
            else if (thumbnails.Count() < 9 && page > 1)
            {
                ViewBag.PrevPage = page - 1;
                ViewBag.NextPageNum = null;
            }
            return View(viewModel);
        }

        [Route("Home/Index/{topicId}/{page?}")]
        public IActionResult Index(string topicId, int page = 1)
        {
            HomeViewModel viewModel = new HomeViewModel();


            List<ThumbnailBusiness> thumbnailBusinesses = _pictureService.GetFilteredByTopicThumbs(topicId,page).ToList();
            var mapperThumbs = new MapperConfiguration(cfg => cfg.CreateMap<ThumbnailBusiness, Thumbnail>()).CreateMapper();
            List<Thumbnail> thumbnails = mapperThumbs.Map<IEnumerable<ThumbnailBusiness>, List<Thumbnail>>(thumbnailBusinesses);

            List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
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
            if (thumbnails.Count() > 9 && page == 1)
            {
                ViewBag.NextPageNum = 2;
            }
            else if (thumbnails.Count() > 9)
            {
                ViewBag.PrevPage = page - 1;
                ViewBag.NextPageNum = page + 1;
            }
            else if (thumbnails.Count() < 9 && page > 1)
            {
                ViewBag.PrevPage = page - 1;
                ViewBag.NextPageNum = null;
            }

            return View(viewModel);

        }

        [HttpPost]
        [Route("Home/Search")]
        public IActionResult Search(string request_text)
        {
            if(request_text!=null)
            {
                HomeViewModel viewModel = new HomeViewModel();


                List<ThumbnailBusiness> thumbnailBusinesses = _pictureService.GetSearchedThumbs(request_text).ToList();
                var mapperThumbs = new MapperConfiguration(cfg => cfg.CreateMap<ThumbnailBusiness, Thumbnail>()).CreateMapper();
                List<Thumbnail> thumbnails = mapperThumbs.Map<IEnumerable<ThumbnailBusiness>, List<Thumbnail>>(thumbnailBusinesses);

                List<TopicBusiness> topicBusinesses = _topicService.GetTopics().ToList();
                var mapperTopics = new MapperConfiguration(cfg => cfg.CreateMap<TopicBusiness, Topic>()).CreateMapper();
                viewModel.Topics = mapperTopics.Map<IEnumerable<TopicBusiness>, List<Topic>>(topicBusinesses);

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
            else
            {
                TempData["Failure"]= "There must be some text in serch request!";
                return RedirectToAction("Index");
            }
            
        }
        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
