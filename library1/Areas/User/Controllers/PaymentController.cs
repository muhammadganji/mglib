using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using library1.Classes;
using library1.Models;
using library1.Services;
using library1.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace library1.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "user")]
    public class PaymentController : Controller
    {
        #region private
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPaymentService _ips;
        private readonly IUserService _ius;
        #endregion

        #region constructor
        public PaymentController(IPaymentService ips, IUserService ius, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _ips = ips;
            _ius = ius;
        }
        #endregion

        #region load
        [HttpGet]
        public IActionResult Payment()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            return View();
        }

        #endregion

        #region Connect to Gatway
        [HttpPost]
        public async Task<IActionResult> Payment(PaymentTransaction pt)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            if (pt.Amount == 0)
            {
                ModelState.AddModelError("AmountError", "مبلغ وارد شده صحیح نیست، مبلغ بایستی بیشتر از 100 تومان باشد");
            }
            if (!ModelState.IsValid)
            {
                return View("Payment", pt);
            }

            // پرداخت واقعی
            //var payment = await new ZarinPal.Payment("merchantId",pt.Amount)

            // قبل از پرداخت :
            var payment = await new ZarinpalSandbox.Payment(pt.Amount).PaymentRequest(pt.Description,
                Url.Action(nameof(PaymentVerify), "Payment", new
                {

                    amount = pt.Amount,
                    description = pt.Description,
                    email = pt.Email,
                    mobile = pt.Mobile
                }
                , Request.Scheme), pt.Email, pt.Mobile);

            // بعد از پرداخت :

            // در صورت موفقیت آمیز بودن درخواست، کاربر به صفحه پرداخت هدایت شود
            // در غیر این صورت باید با خطا مواجه شود
            return payment.Status == 100 ? (IActionResult)Redirect(payment.Link) :
                BadRequest($"خطا در پرداخت  کد : {payment.Status}");
        }

        public async Task<IActionResult> PaymentVerify(int amount, string description, string email, string mobile, string Authority, string Status)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            if (Status == "NOK") return View("failedPayment");

            // ارسال به صفحه خطا
            var verification = await new ZarinpalSandbox.Payment(amount).Verification(Authority);

            // ارسال کد تراکنش جهت نمایش به کاربر
            if (verification.Status != 100) return View("failedView");
            // اطلاعات پرداخت
            var RefId = verification.RefId;

            // اگر اطلاعات درست باشد، باید در دیتابیس ذخیره شود ------------
            DateTimePersian dt = new DateTimePersian();
            string shamsiDate = dt.shamsiDate();
            string shamsiTime = dt.shamsiTime();

            // ثبت اطلاعات پرداخت در دیتابیس
            var result = _ips.PaymentVerify(_ius.GetId(),
                email,
                mobile,
                description,
                amount,
                verification.RefId.ToString());

            if (result)
            {
                ViewBag.TransactionNo = verification.RefId.ToString();
                ViewBag.TransactonDate = shamsiDate;
                ViewBag.TransactionTime = shamsiTime;
                ViewBag.Description = description;
                ViewBag.Amount = amount;

                return View("SuccessfullyPayment");
            }
            else
            {
                return View("failedPayment");
            }
        }
        #endregion
    }
}