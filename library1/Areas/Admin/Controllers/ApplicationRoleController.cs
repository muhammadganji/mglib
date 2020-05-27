using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using library1.Services;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ApplicationRoleController : Controller
    {
        #region Priavte
        private readonly IRoleService _irs;
        #endregion

        #region Constructor
        public ApplicationRoleController(IRoleService irs)
        {
            _irs = irs;
        }
        #endregion

        #region load
        public IActionResult Index()
        {
            var userorles = _irs.getAllUserRole();
            var roles = _irs.getAllRole();
            List<ApplicationRoleViewModel> model = new List<ApplicationRoleViewModel>();
            model = roles.Select(r => new ApplicationRoleViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                NumberOfUsers = userorles.Count(ur => ur.RoleId == r.Id)
            }).ToList();

            return View(model);
        }

        #endregion

        #region Add | Edit
        [HttpGet]
        public async Task<IActionResult> AddEditRole(string id)
        {
            ApplicationRoleViewModel model = new ApplicationRoleViewModel();
            if (!string.IsNullOrEmpty(id)) // Edit
            {
                ApplicationRole applicaitonrole = await _irs.FindByIdAsync(id);
                if (applicaitonrole != null)
                {
                    model.Id = applicaitonrole.Id;
                    model.Name = applicaitonrole.Name;
                    model.Description = applicaitonrole.Description;
                }
                else
                {
                    return RedirectToAction("Index"); // Do nothing
                }
            }

            // Add
            return PartialView("_AddEditApplicationRole", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditRole(string id, ApplicationRoleViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(id); // !
                ApplicationRole applicationrole = isExist ? await _irs.FindByIdAsync(id) : new ApplicationRole
                {
                    // insert

                };

                applicationrole.Name = model.Name;
                applicationrole.Description = model.Description;

                IdentityResult roleResult = isExist ? await _irs.updateAsync(applicationrole) :
                    await _irs.createAsync(applicationrole);

                if (roleResult.Succeeded)
                {
                    return PartialView("_succesfullyresponse", redirectUrl);
                }
            }

            return PartialView("_AddEditApplicationRole", model);
        }

        #endregion

        #region delete
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole ar = await _irs.FindByIdAsync(id);
                if (ar != null)
                {
                    name = ar.Name;
                }
            }
            return PartialView("_deleterole", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id, IFormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole ar = await _irs.FindByIdAsync(id);
                if (ar != null)
                {
                    IdentityResult roleResult = _irs.Delete(ar);
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        #endregion
    }
}