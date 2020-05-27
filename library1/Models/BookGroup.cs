using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models
{
    public class BookGroup
    {
        // Model group books
        [Key]
        public int BookGroupId { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage ="لطفا نام گروه بندی فراموش نشه")]
        public string GropuName { get; set; }

        [Display(Name = "توضیح")]
        [Required(ErrorMessage ="توضیح هم بنویسید لطفا")]
        public string Description { get; set; }

        public virtual ICollection<Book> books { get; set; }

    }
}
