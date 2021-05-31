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
    public class ThumbnailRepository:IRepository<Thumbnail>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public ThumbnailRepository(AppDataContext data)
        {
            _appData = data;
        }

        public Thumbnail Create(Thumbnail item)
        {
            return _appData.Thumbs.Add(item).Entity ;
        }

        public void Delete(string id)
        {
            Thumbnail thum = _appData.Thumbs.Find(id);
            if (thum != null)
            {
                _appData.Thumbs.Remove(thum);
            }
        }

        public IEnumerable<Thumbnail> Find(Func<Thumbnail, bool> predicate)
        {
            return _appData.Thumbs.Where(predicate);
        }

        public IEnumerable<Thumbnail> FindSearch(string searchString)
        {
            List<Thumbnail> thumbnails = (from thum in _appData.Thumbs
                                          join orig in _appData.Pictures on thum.OriginalId equals orig.Id
                                          where orig.Name.Contains(searchString) == true
                                          select thum).ToList();
            return thumbnails;
        }

        public Thumbnail Get(string id)
        {
            return _appData.Thumbs.Find(id);
        }

        public IEnumerable<Thumbnail> GetAll()
        {
            return _appData.Thumbs;
        }

        public void Update(Thumbnail item)
        {
            _appData.Entry(item).State = EntityState.Modified;
        }
    }
}
