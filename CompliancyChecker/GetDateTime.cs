using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetDateTime
{
    class GetDateTime
    {
        public string TodaysDate()
        {
            DateTime dateTime = DateTime.Now;
            string year = dateTime.Year.ToString();
            string month = dateTime.Month.ToString();
            string day = dateTime.Day.ToString();
            string hour = dateTime.Hour.ToString("D2");
            string minute = dateTime.Minute.ToString("D2");
            string second = dateTime.Second.ToString("D2");
            string todaysDate = ("y" + year + "m" + month + "d" + day + "t" + hour + "" + minute + "" + second);
            return todaysDate;
        }

    }
}
