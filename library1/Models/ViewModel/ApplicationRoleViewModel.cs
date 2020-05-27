using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class ApplicationRoleViewModel
    {
        [Key]
        public string Id { get; set; }


        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage ="نام نقش نبایست خالی باشه")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage ="توضیحات هم نباید خالی باشه")]
        public string Description { get; set; }


        [Display(Name="تعداد کاربران")]
        public int NumberOfUsers { get; set; }
    }
}
