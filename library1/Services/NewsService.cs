using library1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public class NewsService : INewsService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly int pagesize = 10;
        #endregion

        #region Constructor
        public NewsService(ApplicationDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Find By Id
        public News FindById(int Id)
        {
            News model = _db.news.Where(n => n.NewsId == Id).SingleOrDefault();
            return model;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<PagingList<News>> load(int page)
        {
            var model = _db.news.AsNoTracking().Select(n => new News()
            {
                NewsId = n.NewsId,
                newTitle = n.newTitle,
                newsConten = n.newsConten,
                newsDate = n.newsDate,
                newsImage = n.newsImage
            }).OrderBy(n => n.NewsId);
            var modelPaging = await PagingList<News>.CreateAsync(model, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<PagingList<News>> search(string fromDate1, string todate1, string searchnews, int page = 1)
        {
            var model = _db.news.AsNoTracking().Select(n => new News
            {
                NewsId = n.NewsId,
                newTitle = n.newTitle,
                newsConten = n.newsConten,
                newsDate = n.newsDate,
                newsImage = n.newsImage
            }).OrderBy(a => a.NewsId);

            var modelPaging = await PagingList<News>.CreateAsync(model, pagesize, page);

            // filter
            if (fromDate1 != null && todate1 == null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<News>.CreateAsync(
                    model.Where(m => m.newsDate.CompareTo(fromDate1) >= 0).OrderBy(a => a.NewsId), pagesize, page);
            }
            if (todate1 != null && fromDate1 == null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<News>.CreateAsync(
                    model.Where(m => m.newsDate.CompareTo(todate1) <= 0).OrderBy(a => a.NewsId), pagesize, page);
            }
            if (todate1 != null && fromDate1 != null)
            {
                modelPaging = await PagingList<News>.CreateAsync(
                    model.Where(m => m.newsDate.CompareTo(todate1) <= 0 && m.newsDate.CompareTo(fromDate1) >= 0).OrderBy(a => a.NewsId), pagesize, page);
            }
            if (searchnews != null)
            {
                modelPaging = await PagingList<News>.CreateAsync(
                    model.Where(m => m.newTitle.Contains(searchnews) || m.newsConten.Contains(searchnews)).OrderBy(a => a.NewsId), pagesize, page);
            }
            return modelPaging;
        }
        #endregion

        #region update
        public void update(News news)
        {
            _db.news.Update(news);
        }
        #endregion

        #region insert
        public void create(News news)
        {
            _db.news.Add(news);
        }

        #endregion

        #region save
        public void save()
        {
            _db.SaveChanges();
        }
        #endregion

        #region delete
        public void delete(News news)
        {
            _db.news.Remove(news);
        }
        #endregion
    }
}
