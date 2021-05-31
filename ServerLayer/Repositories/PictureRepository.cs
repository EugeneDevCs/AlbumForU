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
    public class PictureRepository:IRepository<Picture>
    {
        private readonly AppDataContext _appData;
        //I will send a database object to this constructor
        public PictureRepository(AppDataContext data)
        {
            _appData = data;
        }

        public Picture Create(Picture picture)
        {
            return _appData.Pictures.Add(picture).Entity;
        }

        public void Delete(string id)
        {
            Picture pic = _appData.Pictures.Find(id);
            if(pic != null)
            {
                _appData.Pictures.Remove(pic);
            }

        }

        public IEnumerable<Picture> Find(Func<Picture, bool> predicate)
        {
            return _appData.Pictures.Where(predicate);
        }

        public Picture Get(string id)
        {
            return _appData.Pictures.Find(id);
        }

        public IEnumerable<Picture> GetAll()
        {
            return _appData.Pictures;
        }

        public void Update(Picture picture)
        {
            _appData.Entry(picture).State = EntityState.Modified;
        }
    }
}
