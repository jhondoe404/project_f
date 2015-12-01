using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Helpers
{
    class Utils
    {
        public static string formatServerDate(string serverDate)
        {
            string monthExtracted = serverDate.Substring(5, 2);
            int month = Int32.Parse(monthExtracted);
            string day = serverDate.Substring(8, 2);
            string year = serverDate.Substring(0, 4);
            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            return strMonthName + " " + day + ", " + year;
        }
    }
}
