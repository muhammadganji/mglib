using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using library1.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using library1.Classes;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BookController : Controller
    {
        #region private
        /// <summary>
        /// برای دسترسی به آدرس روت صفحه
        /// </summary>
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBookService _ibs;
        private readonly IUserService _ius;
        #endregion

        #region public
        public DateTimePersian dtPersian = new DateTimePersian();
        #endregion

        #region constructor
        public BookController(IWebHostEnvironment appEnvironment, SignInManager<ApplicationUser> signInManager, IBookService ibs, IUserService ius)
        {
            _signInManager = signInManager;

            _appEnvironment = appEnvironment;
            _ibs = ibs;
            _ius = ius;
        }
        #endregion

        #region load
        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.RootPath = "/upload/thumbnailimage/";

            var model = await _ibs.BookListAsync(page);
            return View(model);
        }
        #endregion

        #region Add 
        [HttpGet]
        public IActionResult AddBook()
        {
            var model = _ibs.AddBook();
            return PartialView("_AddEditbook", model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var model = _ibs.EditBook(id);

            return PartialView("_AddEditbook", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditBook(int BookId, AddEditBookViewModel model, IEnumerable<IFormFile> files, string ImgName)
        {
            if (ModelState.IsValid)
            {
                #region Begin: upload image
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "upload\\normalimage\\");
                var uploadsThubnail = Path.Combine(_appEnvironment.WebRootPath, "upload\\thumbnailimage\\");
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fs = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                            model.Image = filename;
                        }
                        // resize image
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer();
                        img.Resize(uploads + filename, uploadsThubnail + filename);
                    }

                }
                #endregion

                // insert
                if (BookId == 0)
                {
                    _ibs.InsertBook(model);
                    _ibs.Save();

                    return Json(new { status = "success", message = "کتاب با موفقیت ثبت شد" });
                }
                // update
                else
                {
                    if (model.Image == null)
                    {
                        model.Image = ImgName;
                    }
                    _ibs.EditBook(model);
                    _ibs.Save();

                    return Json(new { status = "success", message = "کتاب با موفقیت بروز رسانی شد" });
                }

            }

            #region  Display validation with jquery ajax
            var list = new List<string>();
            foreach (var validation in ViewData.ModelState.Values)
            {
                list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
            }
            #endregion

            #region reload select
            model.BookGroups = _ibs.GetAllBookGroup().Select(bg => new SelectListItem
            {
                Text = bg.GropuName,
                Value = bg.BookGroupId.ToString()
            }).ToList();
            model.Authors = _ibs.GetAllAuthor().Select(a => new SelectListItem
            {
                Text = a.AuthorName,
                Value = a.AuthorId.ToString()
            }).ToList();
            #endregion

            return Json(new { status = "error", error = list });
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var model = await _ibs.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_deletebook", model.BookName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id, IFormCollection form)
        {
            var model = await _ibs.GetByIdAsync(id);

            #region delete image
            var uploads = Path.Combine(_appEnvironment.WebRootPath, "upload\\normalimage\\") + model.Image;
            var uploadsThubnail = Path.Combine(_appEnvironment.WebRootPath, "upload\\thumbnailimage\\") + model.Image;

            if (System.IO.File.Exists(uploads))
            {
                System.IO.File.Delete(uploads);
            }
            if (System.IO.File.Exists(uploadsThubnail))
            {
                System.IO.File.Delete(uploadsThubnail);
            }
            #endregion

            _ibs.DeleteBook(model);
            _ibs.Save();

            return RedirectToAction("Index");
        }
        #endregion

        #region search
        public async Task<IActionResult> SearchBook(string searchbook, int page = 1)
        {
            ViewBag.RootPath = "/upload/thumbnailimage/";
            var model = await _ibs.serachAsync(searchbook, page);

            return View("Index", model);
        }
        #endregion

        #region Details
        [HttpGet]
        [AllowAnonymous] // برای دسترسی همه
        public IActionResult BookDetails(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("NotFounds");
            }
            // get name of user
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }


            ViewBag.imageThumbnail = "/upload/thumbnailimage/";
            ViewBag.normalimage = "/upload/normalimage/";
            ViewBag.normalnews = "/upload/normalnews/";
            ViewBag.thumbnailnews = "/upload/thumbnailnews/";

            var model = new MultiModels();

            model.BookDetail = _ibs.BookDetail(Id);
            model.moreViewBook = _ibs.MoreViewBook();

            _ibs.ViewPlus(Id);
            return View(model);
        }

        #endregion

        #region like
        [AllowAnonymous]
        public async Task<IActionResult> Like(int Id)
        {
            var query = _ibs.GetById(Id);
            if (query == null)
            {
                // برگشت به آخرین اکشن فعال
                return Redirect(Request.Headers["Referer"].ToString());// برو به آخرین اکشن که از اونجا اومده
            }

            // check exist cookies
            if (Request.Cookies["_Khosh"] == null)
            {
                // Create cookies
                Response.Cookies.Append("_Khosh", "," + Id + ",", new CookieOptions() { Expires = DateTime.Now.AddYears(2) });
                // delimiter 12195 -> 12,19,5
                query.BookLikeCount++;
                _ibs.update(query);
                await _ibs.SaveAsync();
                return Json(new { status = "success", message = "رای شما ثبت شد", result = query.BookLikeCount });
            }
            else
            {
                string cookieContent = Request.Cookies["_Khosh"].ToString();
                if (cookieContent.Contains("," + Id + ","))
                {
                    // رای داده
                    return View();
                }
                else
                {
                    // ثبت رای جدید
                    cookieContent += "," + Id + ",";
                    Response.Cookies.Append("_Khosh", cookieContent, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(2) });
                    query.BookLikeCount++;
                    _ibs.update(query);
                    await _ibs.SaveAsync();

                    return Json(new { status = "success", message = "رای شما ثبت شد", result = query.BookLikeCount });
                }
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> DisLike(int Id)
        {
            var query = _ibs.GetById(Id);
            if (query == null)
            {
                return Json(new { status = "success", message = "رای شما ثبت شد", result = query.BookLikeCount });
            }

            // check exist cookies
            if (Request.Cookies["_Khosh"] == null)
            {
                // ایجاد کوکی و ثبت رای
                Response.Cookies.Append("_Khosh", "," + Id + ",", new CookieOptions() { Expires = DateTime.Now.AddYears(2) });
                // delimiter 12195 -> 12,19,5
                query.BookLikeCount--;
                _ibs.update(query);
                await _ibs.SaveAsync();
                return Json(new { status = "success", message = "رای شما ثبت شد", result = query.BookLikeCount });
            }
            else
            {
                string cookieContent = Request.Cookies["_Khosh"].ToString();
                if (cookieContent.Contains("," + Id + ","))
                {
                    // رای داده
                    return View();
                }
                else
                {
                    // ثبت رای جدید
                    cookieContent += "," + Id + ",";
                    Response.Cookies.Append("_Khosh", cookieContent, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(2) });
                    query.BookLikeCount--;
                    _ibs.update(query);
                    await _ibs.SaveAsync();

                    return Json(new { status = "success", message = "رای شما ثبت شد", result = query.BookLikeCount });
                }
            }

            //}
        }

        #endregion

        #region borrow
        [AllowAnonymous]
        [Authorize(Roles = "user")]
        public IActionResult Borrow(int id)
        {
            // linq to sql
            var query = _ibs.GetById(id);
            // check exist book
            if (query == null)
            {
                return RedirectToAction("NotFounds");
            }
            // check remind of this book
            if (query.BookStock == 0)
            {
                return Json(new { status = "fail", message = "موجودی کتاب به اتمام رسیده" });
            }
            else
            {
                // check exist cookies
                if (Request.Cookies["_gharz"] == null)
                {
                    Response.Cookies.Append("_gharz", "," + id + ",", new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Expires = DateTime.Now.AddMinutes(30)
                    });
                    return Json(new { statue = "success", message = "در لیست افزوده شد", basket_count = 1 });
                }
                else
                {
                    // exist in list
                    string cookiecontent = Request.Cookies["_gharz"].ToString();
                    if (cookiecontent.Contains("," + id + ",") == true)
                    {
                        int count_item = cookiecontent.Split(",").Where(r => r != "").Count();
                        return Json(new { statue = "exist", message = "این کتاب در لیست شما وجود دارد", basket_count = count_item });
                    }
                    // not exist in list
                    else
                    {
                        cookiecontent += "," + id + ",";
                        Response.Cookies.Append("_gharz", cookiecontent, new Microsoft.AspNetCore.Http.CookieOptions()
                        {
                            Expires = DateTime.Now.AddMinutes(30)
                        });
                        int count_item = cookiecontent.Split(",").Where(r => r != "").Count();
                        string[] list_id = cookiecontent.Split(",").Where(r => r != "").ToArray();
                        return Json(new { statue = "success", message = "در لیست افزوده شد", basket_count = count_item });
                    }
                }
            }
        }

        #endregion

        #region ListShop
        [AllowAnonymous]
        [Authorize(Roles = "user")]
        public IActionResult ListShop()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            var model = new MultiModels();

            if (Request.Cookies["_gharz"] != null)
            {
                string cookieContent = Request.Cookies["_gharz"].ToString();
                string[] requestBook = cookieContent.Split(",").Where(r => r != "").ToArray();
                // query to sql
                model.listShop = _ibs.GetByIdRange(requestBook);
            }
            return View(model);
        }
        #endregion

        #region delete list shop
        [AllowAnonymous]
        [Authorize(Roles = "user")]
        public IActionResult listShopDelete(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId).FirstName + " " + _ius.GetById(UserId).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId).Wallet;
            }

            string cookieContent = Request.Cookies["_gharz"].ToString();
            string[] listShop = cookieContent.Split(",").Where(r => r != "").ToArray();

            // generic list
            List<string> idlist = new List<string>(listShop);
            idlist.Remove(id.ToString());

            cookieContent = "";
            foreach (var item in idlist)
            {
                cookieContent += "," + item + ",";
            }
            Response.Cookies.Append("_gharz", cookieContent, new Microsoft.AspNetCore.Http.CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(30)
            });
            //--------------
            var model = new MultiModels();

            if (Request.Cookies["_gharz"] != null)
            {
                string[] requestBook = cookieContent.Split(",").Where(r => r != "").ToArray();
                // query to sql
                model.listShop = _ibs.GetByIdRange(requestBook);
            }
            return View("ListShop", model);
        }
        #endregion

        #region requset Book
        [AllowAnonymous]
        [Authorize(Roles = "User")]
        public IActionResult BookRequest(string UserId)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // get name of user
                string UserId_ = _ius.GetId();
                ViewBag.fullname = _ius.GetById(UserId_).FirstName + " " + _ius.GetById(UserId_).LastName;
                // update wallet
                ViewBag.Wallet = _ius.GetById(UserId_).Wallet;
            }
            // -1. check total price is lower wallet of User
            // 0. check request befor (prevent repeat)
            // 1. save in data base
            // 2. clear cookies
            //string UserID = _userManager.GetUserId(HttpContext.User);
            string cookieContent = Request.Cookies["_gharz"].ToString();
            string[] listShop = cookieContent.Split(",").Where(r => r != "").ToArray();
            var model = new MultiModels();

            // step: -1
            if (Request.Cookies["_gharz"] != null)
            {
                // query to sql
                model.listShop = _ibs.GetByIdRange(listShop);
            }
            long totalPrice = model.listShop.Sum(b => b.Price);

            string userid_ = _ius.GetId();
            long wallet_user = _ius.GetById(userid_).Wallet;
            if (totalPrice > wallet_user)
            {
                return Json(new { status = "fail", message = "موجودی شما کافی نیست" });
            }

            // step: 0
            if (Request.Cookies["_gharz"] != null)
            {
                var query = _ibs.BorrowCheck(_ius.GetId(), listShop);
                if (query.Count() > 0)
                {
                    return Json(new { status = "exist", message = "لیست شامل کتابی است که قبلا درخواست داده اید" });
                }
            }

            // step: 1
            // استفاده از ترنزکشن
            _ibs.saveBorrow(listShop, userid_, dtPersian.shamsiDate(), totalPrice);

            // step: 2
            Response.Cookies.Delete("_gharz");
            return Json(new { status = "success", message = "ثبت شد و منتظر تایید نهایی باید" });
            //}
        }
        #endregion

        #region not found
        [AllowAnonymous]
        public IActionResult NotFounds()
        {
            return View("NotFounds");
        }
        #endregion
    }
}