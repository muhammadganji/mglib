using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class AddEditBookViewModel
    {
        // Models book

        [Key]
        public int BookId { get; set; }

        [Display(Name = "اسم کتاب")]
        [Required(ErrorMessage ="لطفا نام کتاب رو حتما بنویسید")]
        public string BookName { get; set; }

        [Display(Name = "خلاصه")]
        [Required(ErrorMessage = "لطفا خلاصه ای از کتاب بنویسید")]
        public string Description { get; set; }

        [Display(Name = "برگ")]
        [Required(ErrorMessage = "لطفا تعداد برگ کتاب رو بنویسید")]
        public int PageNumber { get; set; }

        [Display(Name = "تصویر")]
        //[Required(ErrorMessage = "لطفا تصویر کتاب رو بنویسید")]
        public string Image { get; set; }

        [Display(Name ="قیمت")]
        public int Price { get; set; }

        [Display(Name = "تعداد")]
        public int BookStock { get; set; }

        // --------------------
        [Display(Name = "گروه عنوان")]
        public int BookGroupID { get; set; }

        public List<SelectListItem> BookGroups{ get; set; }

        [Display(Name = "نویسنده عزیز")]
        public int AuthorID { get; set; }

        public List<SelectListItem> Authors { get; set; }

    }
}
