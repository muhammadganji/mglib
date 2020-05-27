using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class ConfirmPhoneViewModel
    {
        public string IdUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="کد پیامک شده را وارد نمایید")]
        public string CodeOTP { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

    }
}
