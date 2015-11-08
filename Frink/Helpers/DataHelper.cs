using Frink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Helpers
{
    class DataHelper
    {
        #region CLASS PARAMETERS



        private static readonly DataHelper  _instance = new DataHelper();
        public ThemeModel                   _themeModel { get; set; }



        #endregion
        #region CLASS CONSTRUCT



        private DataHelper() { }

        public static DataHelper Instance
       {
          get 
          {
             return _instance; 
          }
       }



        #endregion
    }
}
