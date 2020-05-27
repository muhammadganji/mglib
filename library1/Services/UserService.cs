using library1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZarinpalSandbox;
using library1.Models.ViewModel;
using ReflectionIT.Mvc.Paging;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace library1.Services
{
    public class UserService : IUserService
    {
        #region private
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;
        // call User in class
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly static int pagesize = 10;

        #endregion

        #region constructor
        public UserService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _contextAccessor = contextAccessor;
        }
        #endregion

        #region get info
        public string GetId()
        {
            string id = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            return id;
        }

        public ApplicationUser GetById(string Id)
        {
            var model = (from u in _db.Users where u.Id == Id select u).SingleOrDefault();
            return model;
        }

        public async Task<ApplicationUser> GetByIdAsync(string Id)
        {
            var model = await (from u in _db.Users where u.Id == Id select u).SingleOrDefaultAsync();
            return model;
        }
        #endregion

        #region load
        [Obsolete]
        public async Task<PagingList<UserListViewModel>> load(int page)
        {
            var query = _userManager.Users.AsNoTracking().Select(u => new UserListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            }).OrderBy(i => i.Id);
            var modelPaging = await PagingList<UserListViewModel>.CreateAsync(query, pagesize, page);
            return modelPaging;
        }
        #endregion

        #region search
        public async Task<ApplicationRole> FindRoleByIdAsync(string ApplicationRoleId)
        {
            ApplicationRole result = await _roleManager.FindByIdAsync(ApplicationRoleId);
            return result;
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            return user;
        }

        public ApplicationRole Role(ApplicationUser user)
        {
            ApplicationRole role = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single());
            return role;
        }

        public string GetUserRoles(ApplicationUser user)
        {
            string result = _userManager.GetRolesAsync(user).Result.Single();
            return result;
        }

        [Obsolete]
        public async Task<PagingList<UserListViewModel>> searchUser(string searchuser, int page)
        {
            // دستورات صفحه بندی
            var query = _userManager.Users.AsNoTracking().Select(u => new UserListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                fullname = u.FirstName + " " + u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            }).OrderBy(i => i.Id);
            var modelPaging = await PagingList<UserListViewModel>.CreateAsync(query, pagesize, page);

            if (searchuser != "")
            {
                modelPaging = await PagingList<UserListViewModel>.CreateAsync(
                    query.Where(m => m.fullname.Contains(searchuser) || m.PhoneNumber.Contains(searchuser) || m.Email.Contains(searchuser)).OrderBy(u => u.Id), pagesize, page);
            }
            return modelPaging;
        }

        #endregion

        #region insert
        public async Task<IdentityResult> CraeteUserAsync(ApplicationUser user, string password)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<IdentityResult> AddRoleAsync(ApplicationUser user, string name)
        {
            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, name);
            return roleResult;
        }
        #endregion

        #region Update
        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            IdentityResult result = await _userManager.UpdateAsync(user);
            return result;

        }
        #endregion

        #region delete
        public async Task<IdentityResult> RemoveFromUserRoleAsync(ApplicationUser user, string existingRole)
        {
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, existingRole);
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);
            return result;
        }
        #endregion

        #region password
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string oldPassword)
        {
            var result = await _userManager.CheckPasswordAsync(user, oldPassword);
            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }
        #endregion
    }
}
