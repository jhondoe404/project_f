using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Helpers
{
    /**
     * 
     * \brief Class containing helper methods not classified under any other 
     * helper or delegate method.
     * 
     * \author  Ivan Gudelj
     * \date    07.12.2015.
     * \version 1.0
     * \copyright   
     *      This code and information is provided "as is" without warranty of
     *      any kind, either expressed or implied, including but not limited to
     *      the implied warranties of merchantability and/or fitness for a
     *      particular purpose.
     */
    class Utils
    {
        /// <summary>
        ///     Formats server date string into a usable 
        ///     format.
        /// </summary>
        /// <param name="serverDate">Date to be formatted</param>
        /// <returns>mmmm dd, yyyy firnat</returns>
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
