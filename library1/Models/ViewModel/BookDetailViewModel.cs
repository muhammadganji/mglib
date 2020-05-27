using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class BookDetailViewModel
    {
        // Models book

        [Key]
        public int BookId { get; set; }
        [Display(Name = "اسم کتاب")]
        public string BookName { get; set; }

        [Display(Name = "خلاصه")]
        public string Description { get; set; }

        [Display(Name = "صفحه")]
        public int PageNumber { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        public int AuthorID { get; set; }
        [Display(Name = "نویسنده")]
        public string AuthorName { get; set; }

        [Display(Name = "تعداد")]
        public int BookStock { get; set; }

        [Display(Name = "نمایش")]
        public int BookViews { get; set; }

        [Display(Name = "قیمت - تومان")]
        public int Price { get; set; }

        public int BookGroupID { get; set; }

        [Display(Name = "گروه عنوان")]
        public string BookGroupName { get; set; }

        [Display(Name ="لایک")]
        public int BookLikeCount { get; set; }

    }
}
