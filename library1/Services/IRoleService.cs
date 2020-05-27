using library1.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IRoleService
    {
        List<ApplicationRole> getAllRole();
        List<IdentityUserRole<string>> getAllUserRole();
        List<ApplicationRole> load();
        ApplicationRole FindById(string RoleId);
        Task<ApplicationRole> FindByIdAsync(string RoleId);
        Task<IdentityResult> createAsync(ApplicationRole role);
        Task<IdentityResult> updateAsync(ApplicationRole role);
        IdentityResult Delete(ApplicationRole role);
        void save();

    }
}
