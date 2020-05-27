using library1.Models;
using library1.Models.ViewModel;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IBookService
    {
        Task<List<BookListViewModel>> BookListAsync(int page);
        AddEditBookViewModel AddBook();
        void InsertBook(AddEditBookViewModel book);
        AddEditBookViewModel EditBook(int id);
        void EditBook(AddEditBookViewModel book);
        List<BookGroup> GetAllBookGroup();
        List<Author> GetAllAuthor();
        Task<Book> GetByIdAsync(int Id);
        Book GetById(int Id);
        void DeleteBook(Book book);
        Task<PagingList<BookListViewModel>> serachAsync(string searchbook, int page = 1);
        void ViewPlus(int id);
        List<BookDetailViewModel> BookDetail(int Id);
        List<Book> MoreViewBook();
        void update(Book book);
        Task SaveAsync();
        List<Book> GetByIdRange(string[] ListId);
        List<BorrowBook> BorrowCheck(string UserId, string[] listShop);
        void saveBorrow(string[] listShop, string UserID, string dtPersian, long totalPrice);
        void Save();

    }
}
