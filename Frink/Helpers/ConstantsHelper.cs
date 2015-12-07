using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Helpers
{
    /**
     * 
     * \brief Helper class containing all of the constant values used in the project
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
    class ConstantsHelper
    {
        #region RESTFUL API



        public const String API_APP_ID = "1";
        public const String API_HOST_URL= "http://frink-dev.smartfactory.ch:1337/api";

        public const String API_ETAG = "ETag";
        public const String API_ETAG_CACHE_DELIMITER = " =#=! ";

        public const String API_METHOD_MENU =  "/app";
        public const String API_METHOD_LIST = "/entries";



        #endregion
        #region  LOCALE



        public const String LOCALE_PASSWORD = "thisisanawasomecoolpassword";
        public const String LOCALE_KEY_APPLICATION_THEME = "applicationtheme";

        public const String LOCAL_FILE_APPLICATION_THEME = "apptheme.txt";



        #endregion
        #region THEME



        public const String NAVIGATION_TYPE_GRID = "CollectionView";
        public const String NAVIGATION_TYPE_TABLE = "TableView";
        public const String NAVIGATION_TYPE_LOCATION = "LocationView";



        #endregion
    }
}
