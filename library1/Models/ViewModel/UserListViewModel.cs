using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class UserListViewModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name="اسم")]
        public string FirstName { get; set; }

        [Display(Name = "فامیلی")]
        public string LastName { get; set; }

        public string fullname { get; set; }

        [Display(Name="ایمیل")]
        public string Email { get; set; }

        [Display(Name ="موبایل")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "نقش")]
        public string RoleName { get; set; }

    }
}
