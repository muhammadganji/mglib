using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models
{
    public class Author
    {
        // Model Authors
        [Key]
        public int AuthorId { get; set; }
        [Display(Name="اسم نویسنده")]
        [Required(ErrorMessage ="نام نویسنده حتما باید بنویسید")]
        public string AuthorName { get; set; }
        [Display(Name = "توضیح")]
        [Required(ErrorMessage = "توضیحات هم باید بنویسید")]
        public string Description { get; set; }

        public virtual ICollection<Book> books{ get; set; }

    }
}
