using BusinessLogic.BusinessModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPictureService
    {
        IEnumerable<PictureBusiness> GetPictures();
        PictureBusiness GetCeratainPicture(string oroginalId);
        IEnumerable<ThumbnailBusiness> GetThumbs(int page);
        IEnumerable<ThumbnailBusiness> GetFilteredByTopicThumbs(string topicId, int page);
        IEnumerable<ThumbnailBusiness> GetSearchedThumbs(string searchString);
        Task AddPicture(string PictureName, string TopicId, IFormFile Picture, string webrootPath, string currentUserID);
    }
}
