using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library1.Classes;
using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using avanak;
using library1.Services.User;
using library1.Services;
using Microsoft.AspNetCore.Http;

namespace library1.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "user")]
    public class UserProfileController : Controller
    {
        #region private
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iserviceProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        private const int pagesize = 10;

        public readonly string usernameAv = "09167189048";
        public readonly string passwordAv = "123456";
        public readonly int lenghtAv = 4;
        public int serveridAv = 20;

        private static string _number;
        private static int _codeOTP = 0;

        private readonly IUserProfileService _iups;
        private readonly IUserService _ius;
        #endregion

        #region constructor
        public UserProfileController(IUserProfileService iups, IUserService ius, UserManager<ApplicationUser> userManage, ApplicationDbContext context, IServiceProvider iserviceProvider)
        {
            _context = context;
            _iserviceProvider = iserviceProvider;
            _userManager = userManage;

            _iups = iups;
            _ius = ius;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            var modelPaging = await _iups.load(page);
            return View(modelPaging);
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<IActionResult> SearchInRequest(string fromDate1, string todate1, string bookname, int page = 1)
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            var modelPaging = await _iups.searchInRequest(fromDate1, todate1, bookname, page);
            return View("Index", modelPaging);
        }
        #endregion

        #region change password
        [HttpGet]
        public IActionResult ChangePassword()
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassViewModel model)
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                var userId = _ius.GetId();
                user = _ius.GetById(userId);

                if (await _ius.CheckPasswordAsync(user, model.OldPassword))
                {
                    // old password is Corrent
                    await _ius.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    ViewBag.changepass = "رمز عبور شما با موفقیت تغییر کرد";
                    return View(model);
                }
                else
                {
                    // old password is inCorrect
                    ModelState.AddModelError("", "رمز عبور قدیمی صحیح نیست");
                    ViewBag.wrongpass = "رمز قدیمی اشتباه است";
                    return View(model);
                }
            }

            return View(model);
        }
        #endregion

        #region confirm mobile
        [HttpGet]
        public IActionResult ConfirmMobile()
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            var userId = _ius.GetId();
            var query = _ius.GetById(userId);
            if (query != null)
            {
                ConfirmPhoneViewModel model = new ConfirmPhoneViewModel
                {
                    IdUser = query.Id,
                    FirstName = query.FirstName,
                    LastName = query.LastName,
                    PhoneNumber = query.PhoneNumber,
                    PhoneNumberConfirmed = query.PhoneNumberConfirmed
                };
                _number = model.PhoneNumber;
                return View(model);
            }

            string error = "صفحه مورد نظر یافت نشد";
            return View("NotFounds", error);
        }

        [HttpPost]
        public IActionResult ConfirmMobile(ConfirmPhoneViewModel model)
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            if (ModelState.IsValid)
            {
                try
                {
                    // تایید کد وارد شده
                    int codeOtp = Convert.ToInt32(model.CodeOTP);
                    // کد درست است
                    if (codeOtp == _codeOTP)
                    {
                        // تایید شماره در دیتابیس
                        var userId = _ius.GetId();
                        var query = _ius.GetById(userId);

                        if (query != null)
                        {
                            _iups.ConfirmMobile(query);
                            _iups.save();
                            // نمایش تاییدیه
                            ConfirmPhoneViewModel u = new ConfirmPhoneViewModel
                            {
                                IdUser = query.Id,
                                FirstName = query.FirstName,
                                LastName = query.LastName,
                                PhoneNumber = query.PhoneNumber,
                                PhoneNumberConfirmed = query.PhoneNumberConfirmed
                            };
                            _number = u.PhoneNumber;

                            ViewBag.confirmChecked = "شماره موبایل تایید شد";
                            return View(u);
                        }
                    }
                }
                catch
                {
                    // کاراکتر وارد شده غیر عدد است
                    ViewBag.confirmWrong = "کد وارد شده اشتباه است";
                    return View(model);
                }
            }

            return View(model);
        }

        // Call with Ajax
        public async Task<IActionResult> RequestCodeOTP()
        {
            try
            {
                var client = new WebService3SoapClient(new WebService3SoapClient.EndpointConfiguration() { });

                // درخواست کد
                var result = (await client.SendOTPAsync(usernameAv, passwordAv, lenghtAv, _number, "", serveridAv));

                try
                {
                    _codeOTP = Convert.ToInt32(result);
                }
                catch { }

                #region بررسی نتیجه
                if (_codeOTP > 0)
                {
                    return Json(new { status = "success", message = "درخواست انجام شد، منتظر بمانید" });
                }
                else
                {
                    return Json(new { status = "failed", message = "دوباره درخواست دهید" });
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { status = "failed", message = ex.Message + "\n مشکل ارتباط با سرور" });
            }
        }

        #endregion

        #region info profile
        [HttpGet]
        public IActionResult Profile()
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            var userId = _ius.GetId();
            var query = _ius.GetById(userId);
            var model = new UserListViewModel()
            {
                FirstName = query.FirstName,
                LastName = query.LastName,
                Email = query.Email,
                PhoneNumber = query.PhoneNumber,
                PhoneNumberConfirmed = query.PhoneNumberConfirmed
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(UserListViewModel model, IFormCollection form)
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            if (ModelState.IsValid)
            {
                // change phone number
                var userId = _ius.GetId();
                var user = _ius.GetById(userId);
                if(user.PhoneNumber != model.PhoneNumber)
                {
                    model.PhoneNumberConfirmed = false;
                }

                _iups.updateProfile(model);
                _iups.save();

                ViewBag.changeSuccess = "با موفقیت بروز رسانی انجام شد";
                return View(model);
            }
            ViewBag.chanegeFailed = "خطا در بروزرسانی";
            return View(model);
        }

        #endregion
    }
}