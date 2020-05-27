using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public class BookService : IBookService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly IServiceProvider _iServiceProvider;

        private readonly int pagesize = 10;
        #endregion

        #region Constructor
        public BookService(ApplicationDbContext db, IServiceProvider iServiceProvider)
        {
            _db = db;
            _iServiceProvider = iServiceProvider;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<List<BookListViewModel>> BookListAsync(int page)
        {
            // LinqToQueryString
            var query = (from b in _db.books
                         join a in _db.authors on b.AuthorID equals a.AuthorId
                         join bg in _db.bookGroups on b.BookGroupID equals bg.BookGroupId
                         select new BookListViewModel()
                         {
                             BookId = b.BookId,
                             BookName = b.BookName,
                             PageNumber = b.PageNumber,
                             BookImage = b.Image,
                             AuthorId = b.AuthorID,
                             BookGroupId = b.BookGroupID,
                             AuthorName = a.AuthorName,
                             BookGroupName = bg.GropuName,
                             Price = b.Price,
                             BookStock = b.BookStock
                         }).AsNoTracking().OrderBy(a => a.BookId);
            var modelPaging = await PagingList<BookListViewModel>.CreateAsync(query, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region Insert
        public AddEditBookViewModel AddBook()
        {
            AddEditBookViewModel model = new AddEditBookViewModel();
            model.BookGroups = _db.bookGroups.Select(bg => new SelectListItem
            {
                Text = bg.GropuName,
                Value = bg.BookGroupId.ToString()
            }).ToList();

            model.Authors = _db.authors.Select(a => new SelectListItem
            {
                Text = a.AuthorName,
                Value = a.AuthorId.ToString()
            }).ToList();
            return model;
        }

        public void InsertBook(AddEditBookViewModel model)
        {
            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                Book book = new Book
                {
                    BookName = model.BookName,
                    Description = model.Description,
                    Image = model.Image,
                    AuthorID = model.AuthorID,
                    BookGroupID = model.BookGroupID,
                    PageNumber = model.PageNumber,
                    Price = model.Price,
                    BookStock = model.BookStock

                };
                db.books.Add(book);
            }
        }
        #endregion

        #region Edit
        public AddEditBookViewModel EditBook(int id)
        {
            AddEditBookViewModel model = new AddEditBookViewModel();
            model.BookGroups = _db.bookGroups.Select(bg => new SelectListItem
            {
                Text = bg.GropuName,
                Value = bg.BookGroupId.ToString()
            }).ToList();

            model.Authors = _db.authors.Select(a => new SelectListItem
            {
                Text = a.AuthorName,
                Value = a.AuthorId.ToString()
            }).ToList();

            if (id != 0)
            {
                using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    Book book = db.books.Where(b => b.BookId == id).SingleOrDefault();
                    if (book != null)
                    {
                        model.BookId = book.BookId;
                        model.BookName = book.BookName;
                        model.Description = book.Description;
                        model.PageNumber = book.PageNumber;
                        model.Image = book.Image;
                        model.AuthorID = book.AuthorID;
                        model.BookGroupID = book.BookGroupID;
                        model.Price = book.Price;
                        model.BookStock = book.BookStock;
                    }
                }
            }
            return model;
        }

        public void EditBook(AddEditBookViewModel model)
        {
            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                Book book = new Book
                {
                    BookId = model.BookId,
                    BookName = model.BookName,
                    Description = model.Description,
                    Image = model.Image,
                    AuthorID = model.AuthorID,
                    BookGroupID = model.BookGroupID,
                    PageNumber = model.PageNumber,
                    Price = model.Price,
                    BookStock = model.BookStock

                };
                db.books.Update(book);
            }
        }

        public void update(Book book)
        {
            _db.books.Update(book);
        }

        public List<BookGroup> GetAllBookGroup()
        {
            var model = _db.bookGroups.ToList();
            return model;
        }

        public List<Author> GetAllAuthor()
        {
            var model = _db.authors.ToList();
            return model;
        }
        #endregion

        #region GetById
        public async Task<Book> GetByIdAsync(int Id)
        {
            var model = await _db.books.Where(bg => bg.BookId == Id).SingleOrDefaultAsync();
            return model;
        }

        public Book GetById(int Id)
        {
            var model = _db.books.Where(bg => bg.BookId == Id).SingleOrDefault();
            return model;
        }

        public List<Book> GetByIdRange(string[] ListId)
        {
            var model = (from b in _db.books
                         where ListId.Contains(b.BookId.ToString())
                         select new Book
                         {
                             BookId = b.BookId,
                             BookName = b.BookName,
                             Price = b.Price
                         }).ToList();
            return model;
        }
        #endregion

        #region Delete
        public void DeleteBook(Book book)
        {
            _db.books.Remove(book);
        }
        #endregion

        #region Save
        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<PagingList<BookListViewModel>> serachAsync(string searchbook, int page = 1)
        {
            // LinqToQueryString
            var model = (from b in _db.books
                         join a in _db.authors on b.AuthorID equals a.AuthorId
                         join bg in _db.bookGroups on b.BookGroupID equals bg.BookGroupId
                         select new BookListViewModel()
                         {
                             BookId = b.BookId,
                             BookName = b.BookName,
                             PageNumber = b.PageNumber,
                             BookImage = b.Image,
                             AuthorId = b.AuthorID,
                             BookGroupId = b.BookGroupID,
                             AuthorName = a.AuthorName,
                             BookGroupName = bg.GropuName,
                             Price = b.Price,
                             BookStock = b.BookStock
                         }).AsNoTracking().OrderBy(a => a.BookId);
            var modelPaging = await PagingList<BookListViewModel>.CreateAsync(model, pagesize, page);


            if (searchbook != null)
            {
                modelPaging = await PagingList<BookListViewModel>.CreateAsync(
                    model.Where(m => m.BookName.Contains(searchbook) || m.AuthorName.Contains(searchbook) || m.BookGroupName.Contains(searchbook)).OrderBy(a => a.BookId), pagesize, page);
            }
            return modelPaging;
        }
        #endregion

        #region detail
        public List<BookDetailViewModel> BookDetail(int Id)
        {
            var model = (from b in _db.books
                         join a in _db.authors on b.AuthorID equals a.AuthorId
                         join bg in _db.bookGroups on b.BookGroupID equals bg.BookGroupId
                         where b.BookId == Id

                         select new BookDetailViewModel
                         {
                             BookId = b.BookId,
                             Description = b.Description,
                             PageNumber = b.PageNumber,
                             Image = b.Image,
                             BookName = b.BookName,
                             AuthorName = a.AuthorName,
                             BookGroupName = bg.GropuName,
                             BookStock = b.BookStock,
                             BookViews = b.BookViews,
                             BookLikeCount = b.BookLikeCount,
                             Price = b.Price

                         }).ToList();
            return model;
        }

        public List<Book> MoreViewBook()
        {
            var model = (from b in _db.books orderby b.BookViews select b).OrderByDescending(b => b.BookViews).Take(6).ToList();
            return model;
        }

        #endregion

        #region View++
        public void ViewPlus(int id)
        {
            var result = (from b in _db.books where b.BookId == id select b);
            var currentBook = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentBook.BookViews++;
                // تغییر در یک فیلد ، نه در یک سطر
                _db.books.Attach(currentBook);
                _db.Entry(currentBook).State = EntityState.Modified;

                _db.SaveChanges();
            }
        }
        #endregion

        #region borrow
        public List<BorrowBook> BorrowCheck(string UserId, string[] listShop)
        {
            var model = (from b in _db.borrowBooks
                         where listShop.Contains(b.BookId.ToString())
                         && b.UserId == UserId
                         && b.Flag == 1
                         select b).ToList();
            return model;
        }

        public void saveBorrow(string[] listShop, string UserID, string dtPersian, long totalPrice)
        {
            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in listShop)
                        {
                            BorrowBook br = new BorrowBook();
                            try
                            {
                                br.BookId = Convert.ToInt32(item);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }

                            br.UserId = UserID;

                            br.Flag = 1;
                            br.DateTimeRequest = dtPersian;
                            br.Price = (from b in db.books where b.BookId == br.BookId select b.Price).SingleOrDefault();
                            db.borrowBooks.Add(br);
                        }

                        var query_wallet = (from u in db.Users where u.Id == UserID select u).SingleOrDefault();
                        if (query_wallet != null)
                        {
                            query_wallet.Wallet = query_wallet.Wallet - totalPrice;
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {

                    }

                }
            }
        }

        #endregion
    }

}
