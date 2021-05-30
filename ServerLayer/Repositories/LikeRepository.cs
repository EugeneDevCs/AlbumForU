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
    public class LikeRepository : IRepository<Like>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public LikeRepository(AppDataContext data)
        {
            _appData = data;
        }

        public void Create(Like item)
        {
            _appData.Likes.Add(item); ;
        }

        public void Delete(string id)
        {
            Like like = _appData.Likes.Find(id);
            if (like != null)
            {
                _appData.Likes.Remove(like);
            }
        }

        public IEnumerable<Like> Find(Func<Like, bool> predicate)
        {
            return _appData.Likes.Where(predicate);
        }

        public Like Get(string id)
        {
            return _appData.Likes.Find(id);
        }

        public IEnumerable<Like> GetAll()
        {
            return _appData.Likes;
        }

        public void Update(Like item)
        {
            _appData.Entry(item).State = EntityState.Modified;
        }
    }
}
