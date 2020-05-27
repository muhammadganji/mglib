using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class ManageRequestBookViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int BookId { get; set; }

        public byte Flag { get; set; }

        [Display(Name ="وضعیت")]
        public string FlagDescribtion { get; set; }
        
        [Display(Name = "نام کابر")]
        public string UserName { get; set; }

        [Display(Name ="نام کتاب")]
        public string BookName { get; set; }

        [Display(Name ="موجودی")]
        public int BookStock { get; set; }

        [Display(Name = "تاریخ درخوست کاربر")]
        public string DateTimeRequest { set; get; }
        [Display(Name = "تاریخ تایید ادمین")]
        public string DateTimeConfirmAdmin { set; get; }

        [Display(Name = "تاریخ برگشت کتاب")]
        public string DateTimeGiveBack { set; get; }

        [Display(Name ="مبلغ")]
        public long Price { get; set; }

    }
}
