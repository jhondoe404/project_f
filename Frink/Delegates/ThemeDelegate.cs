using Frink.Helpers;
using Frink.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Frink.Delegates
{
    /**
    * 
    * \brief Delegate class handling application theme resources
    * 
    * 
    * \author  Ivan Gudelj
    * \date    09.12.2015.
    * \version 1.0
    * \copyright   
    *      This code and information is provided "as is" without warranty of
    *      any kind, either expressed or implied, including but not limited to
    *      the implied warranties of merchantability and/or fitness for a
    *      particular purpose.
    */
    class ThemeDelegate
    {
        /// <summary>
        ///     Executes all of the refresh theme methods
        /// </summary>
        public static void refreshTheme()
        {
            refreshThemeNavigationIcons();
            refreshThemeColours();
            refreshFonts();
        }



        /// <summary>
        ///     Sets the application statis resource colour with a 
        ///     specific colour from the theme
        /// </summary>
        /// <param name="resource">Resource to be refreshed</param>
        /// <param name="color">Color pattern from the theme to be used</param>
        private static void refreshColourResource(string resource, float[] color)
        {
            (Application.Current.Resources[resource] as SolidColorBrush).Color = Color.FromArgb
                (
                    byte.Parse((color[3] * 255).ToString()),
                    byte.Parse(color[0].ToString()),
                    byte.Parse(color[1].ToString()),
                    byte.Parse(color[2].ToString())
                );
        }


        /// <summary>
        ///     Refreshes all of the theme colours from the resources
        /// </summary>
        public static void refreshThemeColours()
        {
            refreshColourResource("Color1", DataHelper.Instance._themeModel.app.colorScheme.color1);
            refreshColourResource("Color2", DataHelper.Instance._themeModel.app.colorScheme.color2);
            refreshColourResource("Color3", DataHelper.Instance._themeModel.app.colorScheme.color3);
        }


        /// <summary>
        ///     Reformats all of the icon strings into a usable format for the XAML
        /// </summary>
        public static void refreshThemeNavigationIcons()
        {
            ObservableCollection<MenuItemModel> items = DataHelper.Instance._themeModel.menu.items;
            for (int i = 0; i < items.Count; i++)
            {
                if (DataHelper.Instance._themeModel.menu.items[i].icon != null)
                {                    
                    string formattedIcon = DataHelper.Instance._themeModel.menu.items[i].icon.Replace("\\", "&#x") + ";";
                    DataHelper.Instance._themeModel.menu.items[i].icon = Windows.Data.Html.HtmlUtilities.ConvertToText(formattedIcon);
                }
                
            }            
        }        


        /// <summary>
        ///     Sets the singletone instances to the appropriate fonts 
        ///     passed by the theme http request
        /// </summary>
        public static void refreshFonts()
        {
            if (DataHelper.Instance._themeModel != null
                && DataHelper.Instance._themeModel.app != null
                && DataHelper.Instance._themeModel.app.titleFont != null
                && !DataHelper.Instance._themeModel.app.titleFont.Equals(""))
            {
                DataHelper.Instance._head = getFontFamily(DataHelper.Instance._themeModel.app.titleFont);
            }

            if (DataHelper.Instance._themeModel != null
                    && DataHelper.Instance._themeModel.app != null
                    && DataHelper.Instance._themeModel.app.bodyFont != null
                    && !DataHelper.Instance._themeModel.app.bodyFont.Equals(""))
            {
                DataHelper.Instance._body = getFontFamily(DataHelper.Instance._themeModel.app.bodyFont);
            }
        }


        /// <summary>
        ///     Returns appropriate path based on the supplied font name
        /// </summary>
        /// <param name="fontName">Name of the font</param>
        /// <returns>Instanced object based on the supplied font name</returns>
        private static FontFamily getFontFamily(string fontName)
        {
            string fontNameCleared = fontName.Replace(" ", "");
            string route = "Assets/Fonts/" + fontNameCleared + "-Regular.ttf#";
            switch (fontNameCleared)
            {
                case "BreeSerif":
                    route += "Bree Serif";
                    break;
                case "Cabin":
                    route += "Cabin";
                    break;
                case "Cantarell":
                    route += "Cantarell";
                    break;
                case "Cardo":
                    route += "Cardo";
                    break;
                case "Dosis":
                    route += "Dosis";
                    break;
                case "DroidSans":
                    route += "Droid Sans";
                    break;
                case "FjallaOne":
                    route += "Fjalla One";
                    break;
                case "JosefinSans":
                    route += "Josefin Sans";
                    break;
                case "Lato":
                    route += "Lato";
                    break;
                case "Merriweather":
                    route += "Merriweather";
                    break;
                case "OpenSans":
                    route += "Open Sans";
                    break;
                case "Average":
                default:
                    route += "Average";
                    break;
            }

            return new FontFamily(route);
        }
    }
}
