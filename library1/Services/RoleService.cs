using avanak;
using library1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public class RoleService : IRoleService
    {
        #region private
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;
        #endregion

        #region constructor
        public RoleService(IHttpContextAccessor contextAccessor, RoleManager<ApplicationRole> roleManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _db = db;
        }
        #endregion

        #region get info
        public List<ApplicationRole> getAllRole()
        {
            var model = _db.Roles.ToList();
            return model;
        }
        public List<IdentityUserRole<string>> getAllUserRole()
        {
            var model = _db.UserRoles.ToList();
            return model;
        }
        #endregion

        #region load
        public List<ApplicationRole> load()
        {
            var model = _db.Roles.ToList();
            return model;
        }
        #endregion

        #region search
        public ApplicationRole FindById(string RoleId)
        {
            var model = _db.Roles.Where(r => r.Id == RoleId).SingleOrDefault();
            return model;
        }

        public async Task<ApplicationRole> FindByIdAsync(string RoleId)
        {
            var model = await _roleManager.FindByIdAsync(RoleId);
            return model;
        }
        #endregion

        #region insert
        public async Task<IdentityResult> createAsync(ApplicationRole role)
        {
            IdentityResult result =  await _roleManager.CreateAsync(role);
            return result;
        }
        #endregion

        #region update
        public async Task<IdentityResult> updateAsync(ApplicationRole role)
        {
            IdentityResult result = await _roleManager.UpdateAsync(role);
            return result;
        }
        #endregion

        #region Delete
        public IdentityResult Delete(ApplicationRole role)
        {
            IdentityResult result = _roleManager.DeleteAsync(role).Result;
            return result;
        }
        #endregion

        #region save
        public void save()
        {
            _db.SaveChanges();
        }
        #endregion
    }
}
