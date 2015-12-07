using Frink.Helpers;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            if (DataHelper.Instance.themeColor_3 != null)
                Separator.Stroke = DataHelper.Instance.themeColor_3;
        }




        #endregion
        #region CALLBACKS


        private static void UpdateIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e == null || d == null || e.NewValue == e.OldValue)
                return;

            MainNavigationUserControl navigation = (MainNavigationUserControl)d;

            if (navigation == null || e == null || e.NewValue == null)
                return;

            String text = ((string)e.NewValue).Replace("\\", "&#x") + ";";

            navigation.textBlockIcon.Foreground = DataHelper.Instance.themeColor_1;
            if (navigation.textBlockIcon.FontFamily != DataHelper.Instance.fontAwesome)
                navigation.textBlockIcon.FontFamily = DataHelper.Instance.fontAwesome;

            navigation.textBlockIcon.Text = Windows.Data.Html.HtmlUtilities.ConvertToText(text);
#if DEBUG
            Debug.WriteLine("[MainNavigationUserControl][UpdateIcon] {0} icon: {1}", text, navigation.textBlockIcon.Text);
#endif
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
