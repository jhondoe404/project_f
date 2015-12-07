﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Frink.Delegates
{
    /**
     * 
     * \brief Delegate class handling application navigation
     * methods
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
    class NavigationDelegate
    {
        #region BACK BUTTON



        /// <summary>
        ///     Invoked when back button is pressed. It handels the funcitonality 
        ///     of going back to a previous screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }



        #endregion
    }
}