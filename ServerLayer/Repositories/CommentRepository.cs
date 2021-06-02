using Microsoft.EntityFrameworkCore;
using ServerLayer.DataObtaining;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLayer.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public CommentRepository(AppDataContext data)
        {
            _appData = data;
        }
        public Comment Create(Comment comment)
        {
           return _appData.Comments.Add(comment).Entity;
        }

        public void Delete(string id)
        {
            Comment comm = _appData.Comments.Find(id);
            if (comm != null)
            {
                _appData.Comments.Remove(comm);
            }

        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return _appData.Comments.Where(predicate);
        }

        public Comment Get(string id)
        {
            return _appData.Comments.Find(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _appData.Comments;
        }

        public void Update(Comment comment)
        {
            //here I create new instance of comment 
            //to avoid tracking
            Comment updatedComment = _appData.Comments.Find(comment.Id);

            updatedComment.PictureId = comment.PictureId;
            updatedComment.TextBody = comment.TextBody;
            updatedComment.UserId = comment.UserId;
            updatedComment.UserNick = comment.UserNick;

            _appData.Comments.Update(updatedComment);
        }
    }
}
