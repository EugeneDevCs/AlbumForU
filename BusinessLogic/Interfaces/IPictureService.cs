using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface IPictureService
    {
        IEnumerable<PictureBusiness> GetPictures();
        IEnumerable<PictureBusiness> GetCeratainPicture(string oroginalId);
        IEnumerable<ThumbnailBusiness> GetThumbs(int page);
        IEnumerable<ThumbnailBusiness> GetFilteredByTopicThumbs(string topicId, int page);
        IEnumerable<ThumbnailBusiness> GetSearchedThumbs(string searchString);
    }
}
