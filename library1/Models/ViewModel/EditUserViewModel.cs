using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا نام وارد نمایید")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا نام خانوادگی وارد نمایید")]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage ="لطفا ایمیل هم وارد نمایید")]
        //[DataType(DataType.EmailAddress)]
        //[EmailAddress(ErrorMessage = "ایمیل معتبر وارد نمایید")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",ErrorMessage = "ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        //---------------------- show in list

        // for show role in drop-down list
        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "نقش")]
        public string ApplicaitonRoleId { get; set; }
    }
}
