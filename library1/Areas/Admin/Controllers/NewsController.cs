using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using library1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using library1.Services;
using library1.Classes;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {

        #region private
        /// <summary>
        /// برای دسترسی به آدرس روت صفحه
        /// </summary>
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly INewsService _ins;
        #endregion

        #region constructor
        public NewsController(INewsService ins, IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _ins = ins;
        }
        #endregion

        #region load
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.RootPath = "/upload/thumbnailnews/";
            var modelPaging = await _ins.load(page);
            return View(modelPaging);
        }
        #endregion

        #region search
        public async Task<IActionResult> SearchInNews(string fromDate1, string todate1, string searchnews, int page = 1)
        {
            ViewBag.RootPath = "/upload/thumbnailnews/";

            var modelPaging = await _ins.search(fromDate1, todate1, searchnews, page);

            return View("Index", modelPaging);
        }
        #endregion

        #region AddEdit
        [HttpGet]
        public IActionResult AddEditNews(int id)
        {
            var model = new News();
            ViewBag.RootPath = "/upload/thumbnailnews/";

            if (id != 0)
            {
                model = _ins.FindById(id);
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
            }

            DateTimePersian dt = new DateTimePersian();
            ViewBag.sdate = dt.shamsiDate();

            return PartialView("_AddEditnews", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditNews(int NewsId, News model, IEnumerable<IFormFile> files, string ImgName)
        {
            ViewBag.RootPath = "/upload/thumbnailnews/";

            if (ModelState.IsValid)
            {
                #region Begin: upload image
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "upload\\normalnews\\");
                var uploadsThubnail = Path.Combine(_appEnvironment.WebRootPath, "upload\\thumbnailnews\\");
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fs = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                            model.newsImage = filename;
                        }
                        // resize image
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer();
                        img.Resize(uploads + filename, uploadsThubnail + filename);
                    }

                }
                #endregion

                if (NewsId == 0)
                {
                    // insert
                    _ins.create(model);
                    _ins.save();
                    return Json(new { status = "success", message = "اطلاعیه یا خبر با موفقیت ایجاد شد" });
                }
                else
                {
                    // update
                    if (model.newsImage == null)
                    {
                        model.newsImage = ImgName;
                    }
                    _ins.update(model);
                    _ins.save();
                    return Json(new { status = "success", message = "اطلاعیه و خبر  با موفقیت بروز رسانی شد" });
                }

            }

            #region  Display validation with jquery ajax
            var list = new List<string>();
            foreach (var validation in ViewData.ModelState.Values)
            {
                list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
            }
            #endregion

            return Json(new { status = "error", error = list });

        }
        #endregion

        #region delete

        [HttpGet]
        public IActionResult DeleteNews(int id)
        {
            var tblNews = new News();
            tblNews = _ins.FindById(id);//db.news.Where(bg => bg.NewsId == id).SingleOrDefault();
            if (tblNews == null)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_deletenews", tblNews.newTitle);
        }

        [HttpPost]
        public IActionResult DeleteNews(int id, IFormCollection form)
        {
            var model = _ins.FindById(id);//_context.news.Where(b => b.NewsId == id).SingleOrDefault();

            // delete image
            var uploads = Path.Combine(_appEnvironment.WebRootPath, "upload\\normalnews\\") + model.newsImage;
            var uploadsthumbnail = Path.Combine(_appEnvironment.WebRootPath, "upload\\thumbnailnews\\") + model.newsImage;

            if (System.IO.File.Exists(uploads))
            {
                System.IO.File.Delete(uploads);
            }

            if (System.IO.File.Exists(uploadsthumbnail))
            {
                System.IO.File.Delete(uploadsthumbnail);
            }

            _ins.delete(model);
            _ins.save();

            return RedirectToAction("Index");
        }
        #endregion

        #region show
        [HttpGet]
        [AllowAnonymous]
        public IActionResult NewDetails(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("NotFounds");
            }

            var model = _ins.FindById(id);
            if (model == null)
            {
                return RedirectToAction("NotFounds");
            }

            ViewBag.RootPath = "/upload/normalnews/";

            return View(model);
        }
        #endregion

        #region NotFounds
        [HttpGet]
        [AllowAnonymous]
        public IActionResult NotFounds()
        {
            string message = "صفحه مورد نظر یافت نشد";

            return View("NotFounds", message);
        }
        #endregion

    }
}