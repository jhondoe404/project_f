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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Frink.UserControls
{
    public sealed partial class ListItemUserControl : UserControl
    {
        #region CLASS PROPERTIES


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            (
                "Title",
                typeof(string),
                typeof(ListItemUserControl),
                new PropertyMetadata(null, UpdateTitle)
            );

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }


        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register
            (
                "Description",
                typeof(string),
                typeof(ListItemUserControl),
                new PropertyMetadata(null, UpdateDescription)
            );

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }


        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register
            (
                "Source",
                typeof(string),
                typeof(ListItemUserControl),
                new PropertyMetadata(null, UpdateSource)
            );



        #endregion
        #region CLASS CONSTRUCT



        public ListItemUserControl()
        {
            this.InitializeComponent();
            if (DataHelper.Instance.themeColor_3 != null)
                Separator.Stroke = DataHelper.Instance.themeColor_3;

            if (DataHelper.Instance.themeColor_1 != null)
                textBlockDescription.Foreground = DataHelper.Instance.themeColor_1;
        }



        #endregion
        #region CALLBACKS



        private static void UpdateTitle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e == null || d == null || e.NewValue == e.OldValue)
                return;

            ListItemUserControl item = (ListItemUserControl)d;

            if (item == null || item.textBlockTitle == null || e.NewValue == null)
                return;

            item.textBlockTitle.Text = (string)e.NewValue;
#if DEBUG
            Debug.WriteLine("[ListItemUserControl][UpdateTitle] {0}", e.NewValue);
#endif
        }

        private static void UpdateDescription(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e == null || d == null || e.NewValue == e.OldValue)
                return;

            ListItemUserControl item = (ListItemUserControl)d;

            if (item == null || e.NewValue == null)
                return;

            item.textBlockDescription.Text = Utils.formatServerDate((string)e.NewValue);
#if DEBUG
            Debug.WriteLine("[ListItemUserControl][UpdateDescription] {0}", e.NewValue);
#endif
        }


        private static void UpdateSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e == null || d == null || e.NewValue == e.OldValue)
                return;

            ListItemUserControl item = (ListItemUserControl)d;

            if (item == null || e.NewValue == null)
                return;

            BitmapImage bmi = new BitmapImage();
            Uri myUri = new Uri((string)e.NewValue + "?dim720", UriKind.Absolute);
            bmi.CreateOptions = BitmapCreateOptions.None;
            bmi.UriSource = myUri;
            item.imageLoaderImage.Source = bmi;
#if DEBUG
            Debug.WriteLine("[ListItemUserControl][UpdateSource] {0}", e.NewValue);
#endif
        }



        #endregion
    }
}
