using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using library1.Models;
using library1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.Claims;
using library1.Utilities;

namespace library1.Controllers
{
    public class AccountController : Controller
    {
        #region private
        /// <summary>
        /// برای لاگین در identity مورد استفاده قرار میگیرد
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _dbcontex;

        #endregion

        #region constructor
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbcontext, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbcontex = dbcontext;
            _roleManager = roleManager;
        }
        #endregion

        #region Google account
        // //signin-google
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            // user exist ?
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = {
            info
                .Principal
                .FindFirst(ClaimTypes.Name)
                .Value,
            info
                .Principal
                .FindFirst(ClaimTypes.Email)
                .Value
            };

            if (result.Succeeded)
            {
                // login
                return Redirect("/Home/Index");

                //return View(userInfo);
            }
            else
            {
                // signin
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = info
                    .Principal
                    .FindFirst(ClaimTypes.GivenName)
                    .Value,
                    LastName = info
                        .Principal
                        .FindFirst(ClaimTypes.Surname)
                        .Value,
                    Email = info
                        .Principal
                        .FindFirst(ClaimTypes.Email)
                        .Value,
                    UserName = info
                        .Principal
                        .FindFirst(ClaimTypes.Email)
                        .Value,
                    Wallet = 1000
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);

                if (identResult.Succeeded)
                {



                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        // Add role
                        var roleUser = (from r in _dbcontex.Roles where r.Name == "user" select r).SingleOrDefault();
                        ApplicationRole approle = await _roleManager.FindByIdAsync(roleUser.Id);
                        if (approle != null)
                        {
                            IdentityResult roleresult = await _userManager.AddToRoleAsync(user, approle.Name);
                            if (roleresult.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, false);
                                return Redirect("/Home/Index");

                                //return View(userInfo);
                            }
                        }
                    }
                }
                return AccessDenied();
            }
        }

        private IActionResult AccessDenied()
        {
            return View("NotFound", "خطا رخ داده");
        }
        #endregion

        #region signin
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signin(LoginViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.signin = "true";


            // 1. check uniqe email
            // 2. save
            // 3. login

            // step: 1
            var query_email = (from u in _dbcontex.Users where u.Email == model.signin.Email select u).ToList();
            if (query_email.Count == 0)
            {
                // step: 2
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.signin.FirstName,
                    LastName = model.signin.LastName,
                    Email = model.signin.Email,
                    PhoneNumber = model.signin.PhoneNumber,
                    UserName = model.signin.Email
                };
                if (model.signin.Password == null)
                {
                    return View("Login", model);
                }
                IdentityResult result = await _userManager.CreateAsync(user, model.signin.Password);

                if (result.Succeeded)
                {
                    var roleUser = (from r in _dbcontex.Roles where r.Name == "user" select r).SingleOrDefault();
                    ApplicationRole approle = await _roleManager.FindByIdAsync(roleUser.Id);
                    if (approle != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, approle.Name);
                        if (roleresult.Succeeded)
                        {
                            //var userLogin = await _userManager.FindByNameAsync(model.signin.Email);
                            //string UserRole = _userManager.GetRolesAsync(userLogin).Result.Single();
                            await _signInManager.SignInAsync(user, false);
                            //return RedirectToAction(nameof(HomeController.Index), "Home");
                            return Redirect("/Home/Index");
                        }
                    }
                    else
                    {
                        return View("Login", model);
                    }
                }
                else
                {
                    return View("Login", model);
                }

            }
            else
            {
                // wrong password and username
                ViewBag.error = "ایمیل قبلا ثبت شده است";
                return View("Login", model);
            }

            return View("Login", model);

        }
        #endregion

        #region login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Wellcom"] = "ابتدا وارد شوید با احترام";
            ViewBag.signin = "false";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.signin = "false";

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Get role with Database
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    string UserRole = _userManager.GetRolesAsync(user).Result.Single();

                    // correct username and password
                    return RedirectToLocal(returnUrl, UserRole);
                }
                else
                {
                    // wrong password and username
                    ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
                    return View(model);
                }
            }
            // wrong type
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl, string UserRole)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                // اگر یک مسیر از قبل وجود داشت
                return Redirect(returnUrl);
            }
            else
            {
                if (UserRole == "admin")
                {
                    return Redirect("/Admin/User");
                }
                else if (UserRole == "user")
                {
                    return Redirect("/User/UserProfile");
                }
                // اگر مسیر اشتباه بود یا کاربر فقط میخواهد لاگین نماید
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion

        #region LogOut
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            ViewBag.signin = "false";
            if (Request.Cookies["_gharz"] != null)
            {
                Response.Cookies.Delete("_gharz");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
            // return RedirectToAction("Login");
        }
        #endregion

        #region recovery account
        public IActionResult RecoveryEmail()
        {
            SendEmail.Send("persain.ganji@gmail.com", "Test email", "try two");

            return View("Login");
        }
        #endregion
    }
}