using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library1.Models;
using library1.Models.ViewModel;
using library1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ManageTransactionController : Controller
    {

        #region private
        private readonly SignInManager<ApplicationUser> _signInManager;
        private const int pagesize = 10;
        private readonly IUserService _ius;
        private readonly IManageTransactionService _imts;
        #endregion

        #region Constructor
        public ManageTransactionController(IUserService ius, IManageTransactionService imts, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;

            _ius = ius;
            _imts = imts;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            var modelPaging = await _imts.load(page);
            return View(modelPaging);
        }
        #endregion

        #region search
        public async Task<IActionResult> SearchInPaymentTransaction(string fromDate1, string todate1, string searchuser, int page = 1)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            var modelPaging = await _imts.search(fromDate1, todate1, searchuser, page);
            return View("Index", modelPaging);
        }
        #endregion
    }
}