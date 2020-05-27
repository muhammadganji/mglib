using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class BookListViewModel
    {
        public int BookId { get; set; }
        [Display(Name = "اسم کتاب")]
        public string BookName { get; set; }
        [Display(Name = "صفحه")]
        public int PageNumber { get; set; }
        [Display(Name = "تصویر")]
        public string BookImage { get; set; }
        public int AuthorId { get; set; }
        public int BookGroupId { get; set; }
        [Display(Name = "نویسنده")]
        public string AuthorName { get; set; }
        [Display(Name = "عنوان")]
        public string BookGroupName { get; set; }
        [Display(Name ="قیمت")]
        public int Price { get; set; }

        [Display(Name ="تعداد")]
        public int BookStock { get; set; }
    }
}
