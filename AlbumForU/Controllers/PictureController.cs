using AlbumForU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerLayer.DataObtaining;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AlbumForU.Controllers
{
    [Authorize]
    public class PictureController : Controller
    {
        private AppDataContext appData;
        IWebHostEnvironment _appEnvironment;
        public PictureController(AppDataContext context, IWebHostEnvironment appEnvironment)
        {
            appData = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult AddAPicture()
        {
            PictureCreationViewModel viewModel = new PictureCreationViewModel();
            viewModel.Topics = (from topic in appData.Topics
                                select topic).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAPicture(PictureCreationViewModel viewModel)
        {
            if(ModelState.IsValid && viewModel.Picture!=null)
            {
                string currentUserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                string topicName = (from topic in appData.Topics
                                    where topic.Id == viewModel.TopicId
                                    select topic.Name).FirstOrDefault();

                //Check if the directory exists
                string directoryPath = "pictures/originals/" + topicName;
                if (!Directory.Exists(_appEnvironment.WebRootPath + "/" + directoryPath))
                {
                    Directory.CreateDirectory(_appEnvironment.WebRootPath + "/" + directoryPath);
                }
                string picturePath= directoryPath+"/" + viewModel.Picture.FileName;

                //Saving picture to our file system
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + picturePath, FileMode.CreateNew))
                {
                    await viewModel.Picture.CopyToAsync(fileStream);
                }



                //Check if the directory foe thumb exists
                string directoryPathThumb = "pictures/thumbs/" + topicName;
                if (!Directory.Exists(_appEnvironment.WebRootPath + "/" + directoryPathThumb))
                {
                    Directory.CreateDirectory(_appEnvironment.WebRootPath + "/" + directoryPathThumb);
                }

                //Saving resized (width = 500px) thumb to our file system
                string picturePaththumb = directoryPathThumb + "/" + viewModel.Picture.FileName;
                using (var image = Image.Load(viewModel.Picture.OpenReadStream()))
                {
                    int imageWidth = image.Width - (image.Width - 500);// now width = 500 px

                    int percents = imageWidth * 100 / image.Width; // now we know how many percents
                                                                      //we should subtract
                    int imageHeight = percents * image.Height / 100;//and now we have the image height

                    image.Mutate(x => x.Resize(imageWidth, imageHeight));
                    image.SaveAsJpeg(_appEnvironment.WebRootPath + "/" + picturePaththumb);
                }

                string origId = appData.Pictures.Add(new Picture() { Path = picturePath, Name = viewModel.PictureName, TopicId = viewModel.TopicId, UserId = currentUserID, Date = DateTime.Today}).Entity.Id;
                appData.Thumbs.Add(new Thumbnail() { Path = picturePaththumb, OriginalId = origId, TopicId = viewModel.TopicId, UserId = currentUserID, Date = DateTime.Today });
                appData.SaveChanges();

                TempData["Success"] = $"Picture was successfully uploaded!";
                return Redirect("/Home/Index");
            }

            viewModel.Topics = (from topic in appData.Topics
                                select topic).ToList();
            ModelState.AddModelError("", "Fill in all fields!");
            return View(viewModel);
        }

        
        [Route("Picture/CertainPicture/{originalId}")]
        public IActionResult CertainPicture(string originalId)
        {
            PictureViewModel picture = new PictureViewModel();
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            picture.Picture = (from pic in appData.Pictures
                               where pic.Id == originalId
                               select pic).FirstOrDefault();
            picture.Comments = (from cmt in appData.Comments
                             where cmt.PictureId == originalId
                             select cmt).ToList();
            picture.Like = (from like in appData.Likes
                             where like.PictureId == originalId
                             select like).FirstOrDefault();
            picture.appUser = (from user in appData.Users
                               where user.Id == picture.Picture.UserId
                               select user).FirstOrDefault();

            TempData["originalId"] = originalId;
            return View(picture);
        }

        [HttpPost]
        public IActionResult AddAComment(string commentBody)
        {
            if(ModelState.IsValid && commentBody.Length>0)
            {
                Comment comment = new Comment()
                {
                    dateTime = DateTime.Today,
                    UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    UserNick = (from user in appData.Users
                                where user.Id == this.User.FindFirst(ClaimTypes.NameIdentifier).Value
                                select user.Nickname).FirstOrDefault().ToString(),
                    TextBody=commentBody,
                    PictureId= TempData["originalId"].ToString()
                };
                appData.Comments.Add(comment);
                appData.SaveChanges();

            }            
            return Redirect("CertainPicture/" + TempData["originalId"]);
        }
        public IActionResult LikeDislike(int like, string pictId)
        {
            if(like == 1)
            {
                appData.Likes.Add(new Like { DateTime = DateTime.Today, PictureId = pictId, UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value });
                appData.SaveChanges();
            }
            else
            {
                Like removeLike = (from lk in appData.Likes
                                    where lk.PictureId == pictId && lk.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value
                                    select lk).FirstOrDefault();
                appData.Likes.Remove(removeLike);
                appData.SaveChanges();
            }
            return Redirect("CertainPicture/" + pictId);
        }
    }
}

