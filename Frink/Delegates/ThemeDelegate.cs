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
    }
}
