using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly IUserService _ius;
        private const int pagesize = 10;
        #endregion

        #region Constructor
        public UserProfileService(ApplicationDbContext db, IUserService ius)
        {
            _db = db;
            _ius = ius;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<PagingList<ManageRequestBookViewModel>> load(int page)
        {
            var model = (from br in _db.borrowBooks
                         join b in _db.books on br.BookId equals b.BookId
                         join u in _db.Users on br.UserId equals u.Id

                         where u.Id == _ius.GetId()//_userManager.GetUserId(HttpContext.User)

                         select new ManageRequestBookViewModel()
                         {
                             Id = br.BorrowId,
                             BookId = br.BookId,
                             UserId = br.UserId,
                             //UsreName = u.FirstName + " " + u.LastName,
                             //BookStock = b.BookStock,
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


                         }).AsNoTracking().OrderBy(u => u.Flag);

            var modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(model, pagesize, page);

            return modelPaging;
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<PagingList<ManageRequestBookViewModel>> searchInRequest(string fromDate1, string todate1, string bookname, int page)
        {
            var model = (from br in _db.borrowBooks
                         join b in _db.books on br.BookId equals b.BookId
                         join u in _db.Users on br.UserId equals u.Id

                         where u.Id == _ius.GetId()//_userManager.GetUserId(HttpContext.User)

                         select new ManageRequestBookViewModel()
                         {
                             Id = br.BorrowId,
                             BookId = br.BookId,
                             UserId = br.UserId,
                             //UsreName = u.FirstName + " " + u.LastName,
                             //BookStock = b.BookStock,
                             BookName = b.BookName,
                             Flag = br.Flag,
                             DateTimeRequest = br.DateTimeRequest,
                             DateTimeConfirmAdmin = br.DateTimeConfirmAdmin,
                             DateTimeGiveBack = br.DateTimeGiveBack,
                             Price = br.Price,
                             FlagDescribtion = // select case ...
                             (
                             br.Flag == 1 ? "منتظر تایید" :
                             br.Flag == 2 ? "امانت گرفته" :
                             br.Flag == 3 ? "درخواست رد شد" :
                             br.Flag == 4 ? "برگردانده شده" : "نامشخص"
                             )


                         }).AsNoTracking().OrderBy(r => r.Flag);
            var modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(model, pagesize, page);

            // filter
            if (fromDate1 != null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.DateTimeRequest.CompareTo(fromDate1) >= 0).OrderBy(r => r.Flag), pagesize, page);
            }
            if (todate1 != null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.DateTimeRequest.CompareTo(todate1) <= 0).OrderBy(r => r.Flag), pagesize, page);
            }
            if (bookname != null)
            {
                modelPaging = await PagingList<ManageRequestBookViewModel>.CreateAsync(
                    model.Where(m => m.BookName.Contains(bookname)).OrderBy(r => r.Id), pagesize, page);
            }

            return modelPaging;
        }
        #endregion

        #region confirm mobile
        public void ConfirmMobile(ApplicationUser user)
        {
            user.PhoneNumberConfirmed = true;
            _db.Entry(user).State = EntityState.Modified;
        }
        #endregion

        #region update
        public void updateProfile(UserListViewModel user)
        {
            var userId = _ius.GetId();
            var model = _ius.GetById(userId);
            
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

            _db.Users.Update(model);
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
