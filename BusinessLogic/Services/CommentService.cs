using AutoMapper;
using BusinessLogic.BusinessModels;
using BusinessLogic.Interfaces;
using ServerLayer.Interfaces;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public class CommentService: ICommentService
    {
        //this object is to obtain data from server layer
        IDbAccess dbAccess { get; set; }
        public CommentService(IDbAccess db)
        {
            dbAccess = db;
        }
        //here I use automapper to obtain objects from Server layer 
        //and map them to BL models
        public IEnumerable<CommentBusiness> GetComments(string pictureId)
        {
            var mappedData = new MapperConfiguration(config => config.CreateMap<Comment, CommentBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Comment>, List<CommentBusiness>>(dbAccess.Comments.Find(c => c.PictureId == pictureId));
        }

        public void AddAComment(DateTime dt, string UserId, string textBody, string pictureId)
        {
            if (textBody == null)
            {
                throw new ArgumentNullException("Comment is empty!");
            }
            string usernik = dbAccess.Users.Get(UserId).Nickname;
            Comment comment = new Comment()
            {
                dateTime = dt,
                UserId = UserId,
                UserNick = usernik,
                PictureId = pictureId,
                TextBody = textBody
            };
            try
            {
                dbAccess.Comments.Create(comment);
                dbAccess.Save();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }

        public CommentBusiness FindComment(string id)
        {
            if(id==null)
            {
                throw new ArgumentNullException("Id is empty!");
            }

            Comment comment = dbAccess.Comments.Get(id);
            if (comment == null)
            {
                throw new ArgumentNullException("No such comment!");
            }

            return new CommentBusiness
            {
                dateTime = comment.dateTime,
                Id = comment.Id,
                UserNick = comment.UserNick,
                TextBody = comment.TextBody,
                PictureId = comment.PictureId,
                UserId = comment.UserId
            };
        }
        
        public List<CommentBusiness> FindUsersComments(string userid)
        {
            if(userid == null)
            {
                throw new ArgumentNullException("Id is empty!");
            }
            var mappedData = new MapperConfiguration(config => config.CreateMap<Comment, CommentBusiness>()).CreateMapper();
            return mappedData.Map<IEnumerable<Comment>, List<CommentBusiness>>(dbAccess.Comments.Find(c => c.UserId == userid));

            
        }

        public void DeleteComment(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id is empty!");
            }

            dbAccess.Comments.Delete(id);
            dbAccess.Save();
        }
        public void Dispose()
        {
            dbAccess.Dispose();
        }
    }
}
