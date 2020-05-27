using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری را وارد نمایید")]
        public string UserName { get; set; }

        [Display(Name = "رمز")]
        [Required(ErrorMessage = "لطفا رمز عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// for sign in
        /// </summary>
        public UserViewModel signin { get; set; }

    }
}
