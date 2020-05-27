using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library1.Models;
using library1.Models.ViewModel;
using library1.Services;
using library1.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace library1.Areas.User.Controllers
{
    [Area("user")]
    [Authorize(Roles = "user")]
    public class TransactionController : Controller
    {
        #region private
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iserviceProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private const int pagesize = 10;

        private readonly IUserService _ius;
        private readonly ITransactionService _its;
        #endregion

        #region Constructor
        public TransactionController(IUserService ius,ITransactionService its , ApplicationDbContext context, IServiceProvider iserviceProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _iserviceProvider = iserviceProvider;
            _userManager = userManager;

            _ius = ius;
            _its = its;
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

            var modelPaging = await _its.load(page);
            return View(modelPaging);
        }
        #endregion

        #region search
        public async Task<IActionResult> SearchInPaymentTransaction(string fromDate1, string todate1, int page = 1)
        {
            // get name of user
            string UserId = _ius.GetId();
            ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
            // update wallet
            ViewBag.Wallet = _ius.GetById(UserId).Wallet;

            var modelPaging = await _its.SearchInPaymentTransaction(fromDate1, todate1, page);
            return View("Index", modelPaging);
        }
        #endregion

    }
}