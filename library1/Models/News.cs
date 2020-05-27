using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage ="لطفا عنوان وارد نمایید")]
        public string newTitle { get; set; }

        [Display(Name = "متن خبر")]
        [Required(ErrorMessage = "لطفا متن وارد نمایید")]
        public string newsConten { get; set; }

        [Display(Name = "تاریخ")]
        public string newsDate { get; set; }

        [Display(Name = "تصویر")]
        public string newsImage { get; set; }
    }
}
