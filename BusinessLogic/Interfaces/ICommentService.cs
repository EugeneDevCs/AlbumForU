using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentBusiness> GetComments(string pictureId);
        void AddAComment(CommentBusiness comment);
        CommentBusiness FindComment(string id);
        void DeleteComment(string id);

    }
}
