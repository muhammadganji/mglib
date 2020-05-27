using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Classes
{
    public class DateTimePersian
    {
        #region public
        public DateTime today;
        public PersianCalendar pc = new PersianCalendar();
        #endregion

        #region constructor
        public DateTimePersian()
        {
            today = DateTime.Now;
        }
        #endregion

        #region get date
        public string shamsiDate()
        {
            today = DateTime.Now;
            string shamsiDate = pc.GetYear(today).ToString("0000/") + pc.GetMonth(today).ToString("00/") + pc.GetDayOfMonth(today).ToString("00");
            return shamsiDate;
        }
        #endregion

        #region get time
        public string shamsiTime()
        {
            today = DateTime.Now;
            string shamsiTime = string.Format("{0:HH:mm:ss}",
                Convert.ToDateTime(today.TimeOfDay.Hours +
                ":" + today.TimeOfDay.Minutes +
                ":" + today.TimeOfDay.Seconds
                ));
            return shamsiTime;
        }
        #endregion
    }
}
