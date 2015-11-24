using Frink.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Frink.UserControls
{
    public sealed partial class MainNavigationUserControl : UserControl
    {
        #region CLASS PROPERTIES

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }


        public static readonly DependencyProperty IconProperty = DependencyProperty.Register
            (
                "Icon",
                typeof(string),
                typeof(MainNavigationUserControl),
                new PropertyMetadata(null, UpdateIcon)
            );


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            (
                "Title",
                typeof(string),
                typeof(MainNavigationUserControl),
                new PropertyMetadata(null, UpdateTitle)
            );


        #endregion
        #region CLASS CONSTRUCTS



        public MainNavigationUserControl()
        {
            this.InitializeComponent();
        }




        #endregion
        #region CALLBACKS


        private static void UpdateIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e == null || d == null || e.NewValue == e.OldValue)
                return;

            MainNavigationUserControl navigation = (MainNavigationUserControl)d;

            if (navigation == null)
                return;

            String text = ((string)e.NewValue).Replace("\\", "&#x") + ";";

            navigation.textBlockIcon.Foreground = DataHelper.Instance.themeColor_1;
            if (navigation.textBlockIcon.FontFamily != DataHelper.Instance.fontAwesome)
                navigation.textBlockIcon.FontFamily = DataHelper.Instance.fontAwesome;

            navigation.textBlockIcon.Text = Windows.Data.Html.HtmlUtilities.ConvertToText(text);
        }


        private static void UpdateTitle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
 	        if (e == null || d == null || e.NewValue == e.OldValue)
                return;

            MainNavigationUserControl navigation = (MainNavigationUserControl)d;

            if (navigation == null)
                return;

            navigation.textBlockTitle.Text = (string) e.NewValue;
        }



        #endregion
    }
}
