using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class ChangePassViewModel
    {

        public string IdUser { get; set; }

        [Display(Name = "رمز عبور قدیم")]
        [Required(ErrorMessage = "رمز قدیم باید نوشته بشه")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "حداقل طول رمز جدید 6 کاراکتر و حداکثر 20")]
        public string OldPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "رمز باید نوشته بشه")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "حداقل طول رمز 6 کاراکتر و حداکثر 20")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$",ErrorMessage= "ترکیب حروف بزرگ و کوچک و اعداد رعایت کنید")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز جدید")]
        [Required(ErrorMessage = "تکرار رمز عبور جدید هم باید وارد بشه")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید با تکرار شبیه نیستن")]
        public string ConfirmNewPassword { get; set; }
    }
}
