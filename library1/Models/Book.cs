using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models
{
    public class Book
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

        [Display(Name = "تعداد")]
        public int BookStock { get; set; }

        [Display(Name = "نمایش")]
        public int BookViews { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name ="لایک")]
        public int BookLikeCount { get; set; }

        // Foriegn key [Author]
        [Display(Name = "نویسنده")]
        public int AuthorID { get; set; }

        [ForeignKey("AuthorID")]
        public virtual Author authors { get; set; }

        // Foriegn key [Group]
        [Display(Name = "گروه عنوان")]
        public int BookGroupID { get; set; }

        [ForeignKey("BookGroupID")]
        public virtual BookGroup bookgroups { get; set; }

        [Display(Name ="قیمت")]
        public int Price { get; set; }
    }
}
