using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface ILikeService
    {
        IEnumerable<LikeBusiness> GetLikes(string pictureId);
        void ToLikeDisLike(string userId, string picId);
        bool IsLiked(string userId,string pictureId);
        int CountLikes(string pictureId);
        void Dispose();

    }
}
