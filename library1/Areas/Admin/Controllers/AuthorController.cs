using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using library1.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;
using Microsoft.EntityFrameworkCore;
using library1.Services;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AuthorController : Controller
    {
        #region private
        private readonly IAuthorService _ias;
        #endregion

        #region constructor
        public AuthorController(IAuthorService ias)
        {
            _ias = ias;
        }
        #endregion

        #region load
        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            // استفاده از مدل سرویس
            var model = await _ias.PagingAuthorListAsync(page);
            return View(model);

            // حالت چند صفحه
            //var model = _context.authors.AsNoTracking().Include(a => a.books).OrderBy(a => a.AuthorId);
            //var modelPaging = await PagingList<Author>.CreateAsync(model, pagesize, page);
            //return View(modelPaging);

            // حالت ناهماهنگ
            //var model = _context.authors.Include(a => a.books);
            //return View(await model.ToListAsync());

            // حالت عادی
            //List<Author> model = new List<Author>();
            //// Lambda Expression
            //model = _context.authors.Select(a => new Author
            //{
            //    AuthorId = a.AuthorId,
            //    AuthorName = a.AuthorName,
            //    Description = a.Description
            //}).ToList();

            //return View(model);
        }
        #endregion

        #region Add | Edit
        [HttpGet]
        public async Task<IActionResult> AddEditAuthor(int id)
        {
            var author = new Author();
            if (id != 0)  // Edit
            {
                author = await _ias.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    return RedirectToAction("Index");
                }
            }
            return PartialView("_AddEditAuthor", author);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEditAuthor(Author model, int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    // insert
                    _ias.InsertAuthor(model);
                    _ias.Save();
                    // validation with jquery it's not working
                    //return RedirectToAction("Index");
                    return PartialView("_succesfullyresponse", redirectUrl);
                }
                else
                {
                    // update
                    _ias.EditAuthor(model);
                    _ias.Save();
                    // validation with jquery it's not working
                    //return RedirectToAction("Index");
                    return PartialView("_succesfullyresponse", redirectUrl);
                }
            }
            else
            {
                return PartialView("_AddEditAuthor", model);
            }
        }

        #endregion

        #region delete

        [HttpGet]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var tblAuthor = new Author();
            tblAuthor = await _ias.GetAuthorByIdAsync(id);

            if (tblAuthor == null)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_deleteauthor", tblAuthor.AuthorName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAuthor(int id, IFormCollection form)
        {
            var tblAuthor = await _ias.GetAuthorByIdAsync(id);
            _ias.DeleteAuthor(tblAuthor);
            _ias.Save();
            return RedirectToAction("Index");
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<IActionResult> SearvhAuthor(string searchauthor, int page = 1)
        {
            var modelPaging = await _ias.SearchAuthorService(searchauthor, page);
            return View("Index", modelPaging);
        }
        #endregion
    }
}