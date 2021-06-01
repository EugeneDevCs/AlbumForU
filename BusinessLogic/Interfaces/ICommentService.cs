using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentBusiness> GetComments(string pictureId);
        void AddAComment(DateTime dt, string UserId, string textBody, string pictureId);
        CommentBusiness FindComment(string id);
        void DeleteComment(string id);

    }
}
