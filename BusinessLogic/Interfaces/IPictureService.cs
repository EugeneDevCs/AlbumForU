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
        IEnumerable<ThumbnailBusiness> GetThumbs();
        IEnumerable<ThumbnailBusiness> GetSearchedThumbs(string searchString);
    }
}
