using library1.Classes;
using library1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services.User
{
    public class PaymentService : IPaymentService
    {
        #region private
        private readonly ApplicationDbContext _db;
        #endregion

        #region Constructor
        public PaymentService(ApplicationDbContext db) => _db = db;
        #endregion

        #region verify
        public bool PaymentVerify(string UserId,string email, string mobile, string description, int amount,string TransactionNo)
        {
            DateTimePersian dt = new DateTimePersian();
            string shamsiDate = dt.shamsiDate();
            string shamsiTime = dt.shamsiTime();

            using (var database = _db)
            {
                // همه ی دستورات ثبت شود یا، نیمه کاره رها نشود
                using (var transaction = database.Database.BeginTransaction())
                {
                    try
                    {
                        // ثبت اطلاعات تراکنش
                        PaymentTransaction p = new PaymentTransaction();
                        p.TransactonDate = shamsiDate;
                        p.TransactionTime = shamsiTime;
                        p.Description = description;
                        p.Amount = amount;
                        p.Mobile = mobile;
                        p.Email = email;
                        // شماره تراکنش
                        p.TransactionNo = TransactionNo;
                        p.UserId = UserId;
                        database.paymentTransaction.Add(p);

                        // بروزرسانی کیف پول
                        var updateQuery = (from U in database.Users where U.Id == UserId select U).SingleOrDefault();
                        updateQuery.Wallet = updateQuery.Wallet + amount;

                        database.SaveChanges();
                        transaction.Commit();
                        // return success
                        return true;
                    }
                    catch
                    {
                        // return failed
                        return false;
                    }
                }
            }
        }

        #endregion
    }
}
