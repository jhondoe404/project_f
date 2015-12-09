using Frink.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Frink.Helpers
{
    /**
     * 
     * \brief Helper singleton class to contain specific data in the memory 
     * for faster loading and\or usage etc.
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
    class DataHelper
    {
        #region CLASS PARAMETERS



        private static readonly DataHelper              _instance = new DataHelper();
        public ThemeModel                               _themeModel { get; set; }
        public ObservableCollection<ContentItemModel>   _contentItemModel { get; set; }



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
