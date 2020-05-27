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
    public class TransactionService : ITransactionService
    {
        #region private
        private readonly ApplicationDbContext _db;
        private readonly IUserService _ius;
        private const int pagesize = 10;
        #endregion

        #region Constructor
        public TransactionService(ApplicationDbContext db, IUserService ius)
        {
            _db = db;
            _ius = ius;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<PagingList<PaymentTransactionViewModel>> load(int page)
        {
            var model = (from pt in _db.paymentTransaction
                         join u in _db.Users on pt.UserId equals u.Id
                         where u.Id == _ius.GetId()

                         select new PaymentTransactionViewModel
                         {
                             ID = pt.ID,
                             Amount = pt.Amount,
                             Description = pt.Description,
                             Email = pt.Email,
                             Mobile = pt.Mobile,
                             TransactonDate = pt.TransactonDate,
                             TransactionNo = pt.TransactionNo,
                             TransactionTime = pt.TransactionTime,
                             UserId = pt.UserId

                         }).AsNoTracking().OrderBy(p => p.ID);

            var modelPaging = await PagingList<PaymentTransactionViewModel>.CreateAsync(model, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<PagingList<PaymentTransactionViewModel>> SearchInPaymentTransaction(string fromDate1, string todate1, int page)
        {
            var model = (from pt in _db.paymentTransaction
                         join u in _db.Users on pt.UserId equals u.Id
                         where u.Id == _ius.GetId()

                         select new PaymentTransactionViewModel
                         {
                             ID = pt.ID,
                             Amount = pt.Amount,
                             Description = pt.Description,
                             Email = pt.Email,
                             Mobile = pt.Mobile,
                             TransactonDate = pt.TransactonDate,
                             TransactionNo = pt.TransactionNo,
                             TransactionTime = pt.TransactionTime,
                             UserId = pt.UserId,
                         }).AsNoTracking().OrderBy(pt => pt.ID);

            var modelPaging = await PagingList<PaymentTransactionViewModel>.CreateAsync(model, pagesize, page);

            // filter
            if (fromDate1 != null && todate1 == null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<PaymentTransactionViewModel>.CreateAsync(
                    model.Where(m => m.TransactonDate.CompareTo(fromDate1) >= 0).OrderBy(r => r.ID), pagesize, page);
            }
            if (todate1 != null && fromDate1 == null)
            {
                // برای مقایسه ی دو رشته در لامبدا
                modelPaging = await PagingList<PaymentTransactionViewModel>.CreateAsync(
                    model.Where(m => m.TransactonDate.CompareTo(todate1) <= 0).OrderBy(r => r.ID), pagesize, page);
            }
            if (todate1 != null && fromDate1 != null)
            {
                modelPaging = await PagingList<PaymentTransactionViewModel>.CreateAsync(
                    model.Where(m => m.TransactonDate.CompareTo(todate1) <= 0 && m.TransactonDate.CompareTo(fromDate1) >= 0).OrderBy(r => r.ID), pagesize, page);
            }

            return modelPaging;
        }
        #endregion
    }
}
