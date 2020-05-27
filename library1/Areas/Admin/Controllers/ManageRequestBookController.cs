using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using library1.Classes;
using library1.Models;
using library1.Models.ViewModel;
using library1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReflectionIT.Mvc.Paging;

namespace library1.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class ManageRequestBookController : Controller
    {
        #region private
        private readonly IManageRequestBookService _imrb;
        #endregion

        #region constructor
        public ManageRequestBookController(IManageRequestBookService imrb)
        {
            _imrb = imrb;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            var modelPaging = await _imrb.load(page);
            return View(modelPaging);
        }
        #endregion

        #region search
        [Obsolete]
        public async Task<IActionResult> SearchInRequest(string fromDate1, string todate1, string searchuser, int page = 1)
        {
            var modelPaging = await _imrb.search(fromDate1, todate1, searchuser, page);
            return View("Index", modelPaging);
        }
        #endregion

        #region reject request
        [HttpGet]
        public IActionResult RejectRequest(int id)
        {
            var model = _imrb.RejectRequest(id);
            if (model == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.PartialType = 1;
            return PartialView("_RejectRequest", model);
        }


        [HttpPost]
        public IActionResult RejectRequest(int id, IFormCollection form)
        {
            _imrb.RejectRequestConfirm(id);
            ViewBag.PartialType = 1; // reject State
            return RedirectToAction("Index");
        }
        #endregion

        #region Accept

        [HttpGet]
        public IActionResult AcceptRequest(int id)
        {
            var model = _imrb.AcceptRequest(id);

            if (model == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.PartialType = 2;
            return PartialView("_RejectRequest", model);
        }
        [HttpPost]
        public IActionResult AcceptRequest(int id, IFormCollection form)
        {
            _imrb.AcceptRequsetConfirm(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region give Back request
        [HttpGet]
        public IActionResult GiveBackRequest(int id)
        {
            var model = _imrb.givebackRequest(id);

            if (model == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.PartialType = 3;
            return PartialView("_RejectRequest", model);
        }
        [HttpPost]
        public IActionResult GiveBackRequest(int id, IFormCollection form)
        {
            _imrb.givebackRequestConfirm(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}