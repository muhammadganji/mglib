using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models
{
    public class BorrowBook
    {
        [Key]
        public int BorrowId { get; set; }
        [Display(Name ="شناسه کتاب")]
        public int BookId { get; set; }
        [Display(Name ="شناسه کاربر")]
        public string UserId { set; get; }

        // 1: user request
        // 2: confirm admin
        // 3: back the book
        [Display(Name ="وضعیت")]
        public byte Flag { get; set; }
        [Display(Name ="تاریخ درخوست کاربر")]
        public string DateTimeRequest { set; get; }
        [Display(Name = "تاریخ تایید ادمین")]
        public string DateTimeConfirmAdmin { set; get; }

        [Display(Name = "تاریخ برگشت کتاب")]
        public string DateTimeGiveBack { set; get; }

        [Display(Name ="مبلغ")]
        public int Price { get; set; }

    }
}
