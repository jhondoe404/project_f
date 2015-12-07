using Frink.Helpers;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

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
            if (e == null || d == null || e.NewValue == null 
                || ((string) e.NewValue).Trim().Equals("") || e.NewValue == e.OldValue
                || !e.NewValue.ToString().Contains("http"))
                return;

            ListItemUserControl item = (ListItemUserControl)d;

            if (item == null)
                return;

            BitmapImage bmi = new BitmapImage();
            String url = (string)e.NewValue + "?dim720";
#if DEBUG
            Debug.WriteLine("[ListItemUserControl][UpdateSource] URL of the image {0}", url);
#endif
            Uri myUri = new Uri(url, UriKind.Absolute);
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
