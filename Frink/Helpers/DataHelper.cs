using Frink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Frink.Helpers
{
    class DataHelper
    {
        #region CLASS PARAMETERS



        private static readonly DataHelper  _instance = new DataHelper();
        public ThemeModel                   _themeModel { get; set; }
        private SolidColorBrush             _themeColor_1;
        private FontFamily                  _fontawesome;



        #endregion
        #region GETTERS AND SETTERS

        
        public SolidColorBrush themeColor_1
        {
            get 
            {
                if (this._themeColor_1 == null)
                {
                    float[] _color = this._themeModel.app.colorScheme.color1;
                    int opacityText = (int) (_color[3] * 2.55);
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
