using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using library1.Services;
using Microsoft.VisualBasic;

namespace library1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        #region Priavte
        private readonly IUserService _ius;
        private readonly IRoleService _irs;
        #endregion

        #region Constructor
        public UserController(IRoleService irs, IUserService ius)
        {
            _ius = ius;
            _irs = irs;
        }
        #endregion

        #region load

        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> Index(int page = 1)
        {
            var modelPaging = await _ius.load(page);
            return View(modelPaging);
        }

        #endregion

        #region Add
        [HttpGet]
        public IActionResult AddUser()
        {
            UserViewModel model = new UserViewModel();
            model.ApplicationRoles = _irs.getAllRole().Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();


            return PartialView("_AddUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    Email = model.Email

                };

                IdentityResult result = await _ius.CraeteUserAsync(user, model.Password); //await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    ApplicationRole approle = await _ius.FindRoleByIdAsync(model.ApplicaitonRoleId);//await _roleManager.FindByIdAsync(model.ApplicaitonRoleId);
                    if (approle != null)
                    {
                        IdentityResult roleresult = await _ius.AddRoleAsync(user, approle.Name);//await _userManager.AddToRoleAsync(user, approle.Name);
                        if (roleresult.Succeeded)
                        {
                            return PartialView("_succesfullyresponse", redirectUrl);
                        }
                    }
                }
            }

            //UserViewModel model = new UserViewModel();
            model.ApplicationRoles = _irs.getAllRole().Select(r => new SelectListItem //_roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
            return PartialView("_AddUser", model);

        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel model = new EditUserViewModel();
            model.ApplicationRoles = _irs.getAllRole().Select(r => new SelectListItem //_roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            //----------------
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationUser user = await _ius.FindUserByIdAsync(id); //await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.Email = user.Email;
                    model.ApplicaitonRoleId = _ius.Role(user).Id;//_roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id;
                }
            }


            return PartialView("_EditUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _ius.FindUserByIdAsync(id);//await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;

                    string existingRole = _ius.GetUserRoles(user);//_userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = _irs.getAllRole().Single(r => r.Name == existingRole).Id;//_roleManager.Roles.Single(r => r.Name == existingRole).Id;

                    IdentityResult result = await _ius.UpdateUserAsync(user);//await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (existingRoleId != model.ApplicaitonRoleId)
                        {
                            // if Role Changed
                            IdentityResult roleResult = await _ius.RemoveFromUserRoleAsync(user, existingRoleId); //await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                ApplicationRole approle = await _ius.FindRoleByIdAsync(model.ApplicaitonRoleId); //await _roleManager.FindByIdAsync(model.ApplicaitonRoleId);
                                if (approle != null)
                                {
                                    IdentityResult newrole = await _ius.AddRoleAsync(user, approle.Name); //await _userManager.AddToRoleAsync(user, approle.Name);
                                    if (newrole.Succeeded)
                                    {
                                        return PartialView("_succesfullyresponse", redirectUrl);
                                    }
                                }
                            }
                        }

                        // if role not change
                        return PartialView("_succesfullyresponse", redirectUrl);
                    }
                }
            }

            model.ApplicationRoles = _irs.getAllRole().Select(r => new SelectListItem//_roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            return PartialView("_EditUser", model);
        }
        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser au = await _ius.FindUserByIdAsync(id);//await _userManager.FindByIdAsync(id);
                if (au != null)
                {
                    name = au.FirstName + " " + au.LastName;
                }
            }
            return PartialView("_deleteuser", name);
        }

        public async Task<IActionResult> DeleteUser(string id, IFormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser au = await _ius.FindUserByIdAsync(id); //await _userManager.FindByIdAsync(id);
                if (au != null)
                {
                    IdentityResult userResult = await _ius.DeleteUserAsync(au);//await _userManager.DeleteAsync(au);
                    if (userResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        #endregion

        #region search
        [Obsolete]
        public async Task<IActionResult> SearchUser(string searchuser, int page = 1)
        {
            //// دستورات صفحه بندی
            //var query = _userManager.Users.AsNoTracking().Select(u => new UserListViewModel
            //{
            //    Id = u.Id,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    PhoneNumber = u.PhoneNumber,
            //    Email = u.Email
            //}).OrderBy(i => i.Id);
            //var modelPaging = await PagingList<UserListViewModel>.CreateAsync(query, pagesize, page);

            //if (searchuser != "")
            //{
            //    modelPaging = await PagingList<UserListViewModel>.CreateAsync(
            //        query.Where(m => m.fullname.Contains(searchuser) || m.PhoneNumber.Contains(searchuser) || m.Email.Contains(searchuser)).OrderBy(u => u.Id), pagesize, page);
            //}
            //var query = _userManager.Users.AsNoTracking().Select(u => new UserListViewModel
            //{
            //    Id = u.Id,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    PhoneNumber = u.PhoneNumber,
            //    Email = u.Email
            //}).OrderBy(i => i.Id);
            //var modelPaging = await PagingList<UserListViewModel>.CreateAsync(query, pagesize, page);
            //
            //if (searchuser != "")
            //{
            //    modelPaging = await PagingList<UserListViewModel>.CreateAsync(
            //        query.Where(m => m.fullname.Contains(searchuser) || m.PhoneNumber.Contains(searchuser) || m.Email.Contains(searchuser)).OrderBy(u => u.Id), pagesize, page);
            //}
            var modelPaging = await _ius.searchUser(searchuser, page);

            return View("Index", modelPaging);

            // دستورات عادی جستجو
            //List<UserListViewModel> model = new List<UserListViewModel>();
            //// Lambda Expression
            //model = _userManager.Users.Select(u => new UserListViewModel
            //{
            //    Id = u.Id,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    fullname = u.FirstName + " " + u.LastName,
            //    PhoneNumber = u.PhoneNumber,
            //    Email = u.Email

            //}).ToList();

            //if (searchuser != "")
            //{
            //    model = model.Where(m => m.fullname.Contains(searchuser) || m.PhoneNumber.Contains(searchuser) || m.Email.Contains(searchuser)).ToList();
            //}

            //return View("Index", model);
        }
        #endregion
    }
}