using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using library1.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;
using library1.Services;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BookGroupController : Controller
    {
        #region private
        private readonly IBookGroupService _ibgs;
        #endregion

        #region Constructor
        public BookGroupController(IBookGroupService ibgs)
        {
            _ibgs = ibgs;
        }
        #endregion

        #region load
        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            var modelPaging = await _ibgs.load(page);
            return View(modelPaging);
        }
        #endregion

        #region Add | Edit
        [HttpGet]
        public IActionResult AddEditBookGroup(int id)
        {
            var bookgroup = new BookGroup();
            if (id != 0)
            {
                // Edit
                bookgroup = _ibgs.findById(id);
                if (bookgroup == null)
                {
                    // Do nothing
                    return RedirectToAction("Index"); 
                }
            }
            return PartialView("_AddEditbookgroup", bookgroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEditBookGroup(BookGroup model, int id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    // insert
                    _ibgs.create(model);
                    _ibgs.save();
                    return PartialView("_succesfullyresponse", redirectUrl);
                }
                else
                {
                    _ibgs.update(model);
                    _ibgs.save();
                    return PartialView("_succesfullyresponse", redirectUrl);
                }
            }
            else
            {
                return PartialView("_AddEditbookgroup", model);
            }
        }
        #endregion

        #region delete

        [HttpGet]
        public IActionResult DeleteBookgroup(int id)
        {
            var tblBookGroup = new BookGroup();
            tblBookGroup = _ibgs.findById(id);//db.bookGroups.Where(bg => bg.BookGroupId == id).SingleOrDefault();
            if (tblBookGroup == null)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_deletegroup", tblBookGroup.GropuName);
        }

        [HttpPost]
        public IActionResult DeleteBookgroup(int id, IFormCollection form)
        {
            var BookGroup = _ibgs.findById(id);
            _ibgs.delete(BookGroup);
            _ibgs.save();
            return RedirectToAction("Index");
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<IActionResult> SearchBookGroup(string searchbookgroup, int page = 1)
        {
            var modelPaging = await _ibgs.search(searchbookgroup, page);
            return View("Index", modelPaging);
        }
        #endregion
    }
}