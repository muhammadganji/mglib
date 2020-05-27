using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using library1.Areas.Admin.Controllers;
using library1.Areas.User.Controllers;

namespace library1.Controllers
{
    public class HomeController : Controller
    {

        #region private
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _iserviceProvider;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructor
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IServiceProvider iserviceProvider, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _iserviceProvider = iserviceProvider;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        #region load
        [HttpGet]
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var query_find_Name = (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
                if (query_find_Name != null)
                {
                    // get name of user
                    ViewBag.fullname = query_find_Name.FirstName + " " + query_find_Name.LastName;
                    // update wallet
                    ViewBag.Wallet = query_find_Name.Wallet;
                }

            }

            ViewBag.imageThumbnail = "/upload/thumbnailimage/";
            ViewBag.normalimage = "/upload/normalimage/";
            ViewBag.normalnews = "/upload/normalnews/";
            ViewBag.thumbnailnews = "/upload/thumbnailnews/";

            //var model = new MultiModels();
            //model._SliderLastBook = (from b in _context.books orderby b.BookId select b).Take(6).ToList();
            //model._SliderLastBook = (from b in _context.books select b).ToList();

            #region _slider
            var model = new MultiModels();
            // select slider (lambda expresion)

            
            var query = (from b in _context.books
                         orderby b.BookId descending
                         join a in _context.authors on b.AuthorID equals a.AuthorId
                         join bg in _context.bookGroups.Where(bgg => bgg.GropuName == "کودکان") on b.BookGroupID equals bg.BookGroupId
                         select new
                         {
                             b.BookId,
                             b.BookName,
                             b.PageNumber,
                             b.Image,
                             b.AuthorID,
                             b.BookGroupID,
                             a.AuthorName,
                             bg.GropuName
                         }).Take(6).ToList();

            foreach (var item in query)
            {
                BookListViewModel temps = new BookListViewModel();
                temps.BookId = item.BookId;
                temps.BookName = item.BookName;
                temps.PageNumber = item.PageNumber;
                temps.BookImage = item.Image;
                temps.AuthorId = item.AuthorID;
                temps.BookGroupId = item.BookGroupID;
                temps.AuthorName = item.AuthorName;
                temps.BookGroupName = item.GropuName;
                if (temps != null)
                    model._SliderLastBook.Add(temps);
            }

            #endregion

            #region _LastMoreVisit
            //model._LastMoreVisit = (from b in _context.books where b.BookGroupID == 2 orderby b.BookId descending select b).Take(6).ToList();
            model._LastMoreVisit = (from b in _context.books select b).OrderBy(indexBook => indexBook.BookViews).Take(6).ToList();
            #endregion

            #region _LastRegisterUser
            model._LastRegisterUser = (from u in _userManager.Users orderby u.Id descending select u).Take(10).ToList();
            #endregion

            #region _LastNews
            // get last 6 news
            model._LastNews = (from n in _context.news select n).OrderByDescending(m => m.newsDate).Take(6).ToList();
            #endregion

            #region more view
            model.moreViewBook = (from b in _context.books orderby b.BookViews descending select b).Take(6).ToList();
            #endregion

            return View(model);
        }
        #endregion

        #region Search
        [HttpGet]
        public IActionResult Search(string searchType)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                var query_find_Name = (from u in _context.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
                ViewBag.fullname = query_find_Name.FirstName + " " + query_find_Name.LastName;
                // update wallet
                ViewBag.Wallet = query_find_Name.Wallet;
            }

            var model = (from b in _context.books where b.BookName.Contains(searchType) orderby b.BookName descending select b).Take(15).ToList();
            //var model = _context.books.Where(b => b.BookName.Contains(searchType)).Take(15).ToList();
            ViewBag.imageThumbnail = "/upload/thumbnailimage/";
            ViewBag.normalimage = "/upload/normalimage/";
            ViewBag.seachText = searchType;

            return View("Search", model);
        }
        #endregion

        #region Go To panel
        public IActionResult GoToPanel()
        {
            // Get role with Database
            if (User.IsInRole("admin"))
            {
                //return RedirectToAction(nameof(UserController.Index), "User");

                return Redirect("/Admin/User");
            }
            else if (User.IsInRole("user"))
            {
                //return RedirectToAction(nameof(UserProfileController.Index), "UserProfile");

                return Redirect("/User/UserProfile");
            }
            else
            {
                return View("Index");
                // return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
