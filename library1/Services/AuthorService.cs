using AspNetCore;
using library1.Classes;
using library1.Models;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public class AuthorService : IAuthorService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly int pagesize = 10;
        #endregion

        #region constructor
        public AuthorService(ApplicationDbContext db)
        {
            _db = db;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<List<Author>> PagingAuthorListAsync(int page)
        {
            var model = _db.authors.AsNoTracking().Include(a => a.books).OrderBy(a => a.AuthorId);
            var modelPaging = await PagingList<Author>.CreateAsync(model, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<List<Author>> SearchAuthorService(string searchauthor, int page)
        {
            var model = _db.authors.AsNoTracking().Include(a => a.books).OrderBy(a => a.AuthorId);
            var modelPaging = await PagingList<Author>.CreateAsync(model, pagesize, page);
            // filter
            if (searchauthor != null)
            {
                modelPaging = await PagingList<Author>.CreateAsync(
                    model.Where(m => m.AuthorName.Contains(searchauthor) || m.Description.Contains(searchauthor)).OrderBy(a => a.AuthorId), pagesize, page);
                //model = model.Where(m => m.AuthorName.Contains(searchauthor) || m.Description.Contains(searchauthor)).Include(a => a.books);
            }
            return modelPaging;
        }
        #endregion

        #region Get By Id
        public async Task<Author> GetAuthorByIdAsync(int Id)
        {
            Author author = new Author();
            author = await _db.authors.Where(a => a.AuthorId == Id).SingleOrDefaultAsync();
            return author;
        }
        #endregion

        #region Edit
        public void EditAuthor(Author author)
        {
            _db.Entry(author).State = EntityState.Modified;
        }
        #endregion

        #region Add
        public void InsertAuthor(Author author)
        {
            _db.authors.Add(author);
        }
        #endregion

        #region Delete
        public void DeleteAuthor(Author author)
        {
            _db.Entry(author).State = EntityState.Deleted;
        }
        #endregion

        #region Save
        public void Save()
        {
            _db.SaveChanges();
        }
        #endregion
    }
}
