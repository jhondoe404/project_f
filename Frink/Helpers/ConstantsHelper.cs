using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Helpers
{
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
    }
}
