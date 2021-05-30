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

        public void AddAComment(CommentBusiness commentModel)
        {
            if(commentModel == null)
            {
                throw new ArgumentNullException("Comment is empty!");
            }

            Comment comment = new Comment() { 
                dateTime = commentModel.dateTime,
                UserId = commentModel.UserId,
                UserNick = commentModel.UserNick,
                PictureId=commentModel.PictureId,
                TextBody=commentModel.TextBody
            };

            dbAccess.Comments.Create(comment);

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

        public void DeleteComment(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id is empty!");
            }

            dbAccess.Comments.Delete(id);
        }
    }
}
