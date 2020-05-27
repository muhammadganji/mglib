using Grpc.Core;
using library1.Classes;
using library1.Models;
using library1.Models.ViewModel;
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
    public class ManageRequestBookService : IManageRequestBookService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly IServiceProvider _iServiceProvider;
        private readonly int pagesize = 10;
        #endregion

        #region Constructor
        public ManageRequestBookService(ApplicationDbContext db, IServiceProvider iServiceProvider)
        {
            _db = db;
            _iServiceProvider = iServiceProvider;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<PagingList<ManageRequestBookViewModel>> load(int page)
        {
            var model = (from br in _db.borrowBooks
                         join b in _db.books on br.BookId equals b.BookId
                         join u in _db.Users on br.UserId equals u.Id

                         select new ManageRequestBookViewModel
                         {
                             Id = br.BorrowId,
                             BookId = br.BookId,
                             UserId = br.UserId,
                             UserName = u.FirstName + " " + u.LastName,
                             BookStock = b.BookStock,
                             BookName = b.BookName,
                             Flag = br.Flag,
                             DateTimeRequest = br.DateTimeRequest,
                             DateTimeConfirmAdmin = br.DateTimeConfirmAdmin,
                             DateTimeGiveBack = br.DateTimeGiveBack,
                             Price = br.Price,
                             FlagDescribtion = // select case ...
                             (
                             br.Flag == 1 ? "درخواست امانت" :
                             br.Flag == 2 ? "به امانت برده" :
                             br.Flag == 3 ? "رد درخواست" :
                             br.Flag == 4 ? "برگردانده شده" : "نامشخص"
                             )


                         }).AsNoTracking().OrderBy(r => r.Flag);

            var modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(model, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<PagingList<ManageRequestBookViewModel>> search(string fromDate1, string todate1, string searchuser, int page)
        {
            var model = (from br in _db.borrowBooks
                         join b in _db.books on br.BookId equals b.BookId
                         join u in _db.Users on br.UserId equals u.Id

                         select new ManageRequestBookViewModel
                         {
                             Id = br.BorrowId,
                             BookId = br.BookId,
                             UserId = br.UserId,
                             UserName = u.FirstName + " " + u.LastName,
                             BookStock = b.BookStock,
                             BookName = b.BookName,
                             Flag = br.Flag,
                             DateTimeRequest = br.DateTimeRequest,
                             DateTimeConfirmAdmin = br.DateTimeConfirmAdmin,
                             DateTimeGiveBack = br.DateTimeGiveBack,
                             Price = br.Price,
                             FlagDescribtion = // select case ...
                             (
                             br.Flag == 1 ? "درخواست امانت" :
                             br.Flag == 2 ? "به امانت برده" :
                             br.Flag == 3 ? "رد درخواست" :
                             br.Flag == 4 ? "برگردانده شده" : "نامشخص"
                             )


                         }).AsNoTracking().OrderBy(r => r.Flag);

            var modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(model, pagesize, page);

            // filter
            if (fromDate1 != null && todate1 == null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.DateTimeRequest.CompareTo(fromDate1) >= 0).OrderBy(r => r.Flag), pagesize, page);
            }
            if (todate1 != null && fromDate1 == null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.DateTimeRequest.CompareTo(todate1) <= 0).OrderBy(r => r.Flag), pagesize, page);
            }
            if (todate1 != null && fromDate1 != null)
            {
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.DateTimeRequest.CompareTo(todate1) <= 0 && m.DateTimeRequest.CompareTo(fromDate1) >= 0).OrderBy(r => r.Flag), pagesize, page);
            }
            if (searchuser != null)
            {
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.UserName.Contains(searchuser)).OrderBy(r => r.Flag), pagesize, page);
            }
            return modelPaging;
        }
        #endregion

        #region reject
        public List<ManageRequestBookViewModel> RejectRequest(int id)
        {
            List<ManageRequestBookViewModel> model = new List<ManageRequestBookViewModel>();
            model = (from br in _db.borrowBooks
                     join b in _db.books on br.BookId equals b.BookId
                     join u in _db.Users on br.UserId equals u.Id

                     where br.BorrowId == id

                     select new ManageRequestBookViewModel
                     {
                         UserName = u.FirstName + " " + u.LastName,
                         BookName = b.BookName
                     }).ToList();
            return model;
        }

        public void RejectRequestConfirm(int id)
        {
            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                //// تاریخ شمسی
                DateTimePersian dt = new DateTimePersian();
                string shamsiDate = dt.shamsiDate();

                using (var transaction = db.Database.BeginTransaction())
                {
                    // singleOrDefualt()  اولین درخواست با این مشخصات دیدی بهم تحویل بده
                    var query = (from br in db.borrowBooks where br.BorrowId == id select br);
                    var result = query.SingleOrDefault();
                    if (query.Count() != 0)
                    {
                        result.Flag = 3;
                        result.DateTimeConfirmAdmin = shamsiDate;
                        db.borrowBooks.Attach(result);
                        db.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        // برگشت مبلغ به کیف کاربر
                        var query_back_money = (from u in db.Users where u.Id == result.UserId select u).SingleOrDefault();
                        query_back_money.Wallet += result.Price;

                    }
                    db.SaveChanges();
                    transaction.Commit();
                }

            }
        }
        #endregion

        #region Accept
        public List<ManageRequestBookViewModel> AcceptRequest(int id)
        {
            List<ManageRequestBookViewModel> model = new List<ManageRequestBookViewModel>();
            model = (from br in _db.borrowBooks
                     join b in _db.books on br.BookId equals b.BookId
                     join u in _db.Users on br.UserId equals u.Id

                     where br.BorrowId == id

                     select new ManageRequestBookViewModel
                     {
                         UserName = u.FirstName + " " + u.LastName,
                         BookName = b.BookName
                     }).ToList();
            return model;
        }

        public void AcceptRequsetConfirm(int id)
        {
            DateTimePersian dt = new DateTimePersian();
            string shamsiDate = dt.shamsiDate();

            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                // singleOrDefualt()  اولین درخواست با این مشخصات دیدی بهم تحویل بده
                var query = (from br in db.borrowBooks where br.BorrowId == id select br);
                var result = query.SingleOrDefault();
                // کم کردن موجودی
                var findBook = (from b in db.books where b.BookId == result.BookId select b);
                var resultBook = findBook.SingleOrDefault();
                if (findBook.Count() != 0)
                {
                    resultBook.BookStock--;
                    db.books.Attach(resultBook);
                    db.Entry(resultBook).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                // به مانت بردن
                if (query.Count() != 0)
                {
                    result.Flag = 2;
                    result.DateTimeConfirmAdmin = shamsiDate;
                    db.borrowBooks.Attach(result);
                    db.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region give back
        public List<ManageRequestBookViewModel> givebackRequest(int id)
        {
            List<ManageRequestBookViewModel> model = new List<ManageRequestBookViewModel>();
            model = (from br in _db.borrowBooks
                     join b in _db.books on br.BookId equals b.BookId
                     join u in _db.Users on br.UserId equals u.Id

                     where br.BorrowId == id

                     select new ManageRequestBookViewModel
                     {
                         UserName = u.FirstName + " " + u.LastName,
                         BookName = b.BookName
                     }).ToList();
            return model;
        }

        public void givebackRequestConfirm(int id)
        {
            using (var db = _iServiceProvider.GetRequiredService<ApplicationDbContext>())
            {

                // تاریخ شمسی
                DateTimePersian dt = new DateTimePersian();
                string shamsiDate = dt.shamsiDate();

                var query = (from br in db.borrowBooks where br.BorrowId == id select br);
                var result = query.SingleOrDefault();
                // کم کردن موجودی
                var findBook = (from b in db.books where b.BookId == result.BookId select b);
                var resultBook = findBook.SingleOrDefault();
                if (findBook.Count() != 0)
                {
                    resultBook.BookStock++;
                    db.books.Attach(resultBook);
                    db.Entry(resultBook).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                // به مانت بردن
                if (query.Count() != 0)
                {
                    result.Flag = 4;
                    result.DateTimeGiveBack = shamsiDate;
                    db.borrowBooks.Attach(result);
                    db.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }
        #endregion
    }
}
