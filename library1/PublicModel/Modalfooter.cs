using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.PublicModel
{
    public class Modalfooter
    {
        public string SubmitButtonText { get; set; } = "تایید";
        public string CancelButtonText { get; set; } = "برگشت";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";

        public bool OnlyCancelButton { get; set; }
    }
}
