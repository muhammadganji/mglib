using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری باید نوشته بشه")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز باید نوشته بشه")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "حداقل طول رمز 6 کاراکتر و حداکثر 20")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز")]
        [Required(ErrorMessage = "تکرار رمز عبور هم باید وارد بشه")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="رمز عبور با تکرار شبیه نیستن")]
        public string ConfirmPassword { get; set; }

        //[Display(Name = "اسم کامل قشنگتون")]
        //[Required(ErrorMessage = "اسم باید باشه")]
        //public string Name { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "اسم تون بنویسید")]
        public string FirstName { get; set; }
        
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "فامیلی تون بنویسید")]
        public string LastName { get; set; }

        [Display(Name = "موبایل")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل تون")]
        [Required(ErrorMessage = "ایمیل هم باید نوشته بشه")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //---------------------- show in list

        // for show role in drop-down list
        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "نقش")]
        public string ApplicaitonRoleId { get; set; }
    }
}
