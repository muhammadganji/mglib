using library1.Models;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public class BookGroupService : IBookGroupService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly int pagesize = 10;
        #endregion

        #region constructor
        public BookGroupService(ApplicationDbContext db)
        {
            _db = db;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<PagingList<BookGroup>> load(int page)
        {
            var model = _db.bookGroups.AsNoTracking().Include(b => b.books).OrderBy(a => a.BookGroupId);
            var modelPaging = await PagingList<BookGroup>.CreateAsync(model, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region find by id
        public BookGroup findById(int id)
        {
            var model = _db.bookGroups.Where(bg => bg.BookGroupId == id).SingleOrDefault();
            return model;
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<PagingList<BookGroup>> search(string searchbookgroup, int page)
        {
            var model = _db.bookGroups.AsNoTracking().Include(b => b.books).OrderBy(a => a.BookGroupId);
            var modelPaging = await PagingList<BookGroup>.CreateAsync(model, pagesize, page);
            if (searchbookgroup != null)
            {
                modelPaging = await PagingList<BookGroup>.CreateAsync(
                    model.Where(m => m.GropuName.Contains(searchbookgroup) || m.Description.Contains(searchbookgroup)).OrderBy(a => a.BookGroupId), pagesize, page);
            }
            return modelPaging;
        }
        #endregion

        #region create
        public void create(BookGroup bookgroup)
        {
            _db.bookGroups.Add(bookgroup);
        }
        #endregion

        #region update
        public void update(BookGroup bookgroup)
        {
            _db.bookGroups.Update(bookgroup);
        }
        #endregion

        #region delete
        public void delete(BookGroup bookgroup)
        {
            _db.bookGroups.Remove(bookgroup);
        }
        #endregion

        #region save
        public void save()
        {
            _db.SaveChanges();
        }
        #endregion

    }
}
