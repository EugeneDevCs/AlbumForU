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
        void Delete(string id, string webrootPath);
        IEnumerable<ThumbnailBusiness> GetSearchedThumbs(string searchString);
        IEnumerable<ThumbnailBusiness> GetUserThumbs(string userId);
        Task AddPicture(string PictureName, string TopicId, IFormFile Picture, string webrootPath, string currentUserID);
        void Update(PictureBusiness picture);
        void Dispose();
    }
}
