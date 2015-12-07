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
        private SolidColorBrush                         _themeColor_1;
        private SolidColorBrush                         _themeColor_2;
        private SolidColorBrush                         _themeColor_3;
        private FontFamily                              _fontawesome;
        public ObservableCollection<ContentItemModel>   _contentItemModel { get; set; }



        #endregion
        #region GETTERS AND SETTERS

        
        public SolidColorBrush themeColor_1
        {
            get 
            {
                if (this._themeColor_1 == null)
                {
                    float[] _color = this._themeModel.app.colorScheme.color1;
                    int opacityText = (int) (_color[3] * 255);
                    this._themeColor_1 = new SolidColorBrush(Color.FromArgb
                        (
                            byte.Parse(opacityText.ToString()),
                            byte.Parse(_color[0].ToString()),
                            byte.Parse(_color[1].ToString()),
                            byte.Parse(_color[2].ToString())
                        ));
                }

                return this._themeColor_1; 
            }
            set { this._themeColor_1 = value; }
        }


        public SolidColorBrush themeColor_2
        {
            get
            {
                if (this._themeColor_2 == null)
                {
                    float[] _color = this._themeModel.app.colorScheme.color2;
                    int opacityText = (int)(_color[3] * 255);
                    this._themeColor_2 = new SolidColorBrush(Color.FromArgb
                        (
                            byte.Parse(opacityText.ToString()),
                            byte.Parse(_color[0].ToString()),
                            byte.Parse(_color[1].ToString()),
                            byte.Parse(_color[2].ToString())
                        ));
                }

                return this._themeColor_2;
            }
            set { this._themeColor_2 = value; }
        }


        public SolidColorBrush themeColor_3
        {
            get
            {
                if (this._themeColor_3 == null)
                {
                    float[] _color = this._themeModel.app.colorScheme.color3;
                    int opacityText = (int)(_color[3] * 255);
                    this._themeColor_3 = new SolidColorBrush(Color.FromArgb
                        (
                            byte.Parse(opacityText.ToString()),
                            byte.Parse(_color[0].ToString()),
                            byte.Parse(_color[1].ToString()),
                            byte.Parse(_color[2].ToString())
                        ));
                }

                return this._themeColor_3;
            }
            set { this._themeColor_3 = value; }
        }


        public FontFamily fontAwesome
        {
            get
            {
                if (this._fontawesome == null) { this._fontawesome = new FontFamily("/Assets/Fonts/fontawesome-webfont.ttf#FontAwesome"); }
                return this._fontawesome;
            }
            set { this._fontawesome = value;  }
        }

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
