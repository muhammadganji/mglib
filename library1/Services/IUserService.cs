using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(string Id);
        ApplicationUser GetById(string Id);
        string GetId();
        Task<PagingList<UserListViewModel>> load(int page);
        Task<ApplicationRole> FindRoleByIdAsync(string Id);
        Task<ApplicationUser> FindUserByIdAsync(string Id);
        Task<PagingList<UserListViewModel>> searchUser(string searchuser, int page);
        string GetUserRoles(ApplicationUser user);
        Task<IdentityResult> CraeteUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddRoleAsync(ApplicationUser user, string name);
        ApplicationRole Role(ApplicationUser user);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> RemoveFromUserRoleAsync(ApplicationUser user, string existingRole);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string oldPassword);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);

    }
}
