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
            return View();
        }
    }
}

