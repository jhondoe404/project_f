using Frink.Helpers;
using Frink.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
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
    public sealed partial class ContentDetailsUserControl : UserControl
    {
        #region CLASS PARAMETERS



        private double          originalHeight;
        private ResourceLoader  rl;



        #endregion
        #region CLASS CONSTRUCT



        public ContentDetailsUserControl()
        {
            this.InitializeComponent();
            init(null);
        }

        public ContentDetailsUserControl(ContentItemModel item)
        {
            this.InitializeComponent();
            init(item);
        }


        /// <summary>
        ///     Initial setup method
        /// </summary>
        /// <param name="item">data to be loaded into the view</param>
        private void init(ContentItemModel item)
        {
            if (rl == null)
                rl = new ResourceLoader();

            if (item != null)
            {
                if (item.type == ContentItemModel.CONTENT_ITEM_TABLE)
                {
                    loadImage(item.picture);
                    assertTextBlock(textBlockSubtitle, item.title);
                    assertTextBlock(textBlockDate, item.createdAt);
                    assertTextBlock(textBlockContent, item.text);
                }
                else if (item.type == ContentItemModel.CONTENT_ITEM_GRID)
                {
                    loadImage(item.images[0].picture);
                    assertTextBlock(textBlockSubtitle, item.images[0].title);
                    textBlockDate.Visibility = Visibility.Collapsed;
                    assertTextBlock(textBlockContent, item.images[0].caption);
                }

                if (textBlockSubtitle.FontFamily == null || textBlockSubtitle.FontFamily != DataHelper.Instance._body)
                {
                    textBlockSubtitle.FontFamily = DataHelper.Instance._body;
                    textBlockContent.FontFamily = DataHelper.Instance._body;
                }
            }
        }



        #endregion
        #region EVENT HANDLERS



        private void imageHeader_ImageOpened(object sender, RoutedEventArgs e)
        {
            LoadingPanel.Visibility = Visibility.Collapsed;
            originalHeight = imageHeader.ActualHeight;
        }

        private void imageHeader_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            showMessage(rl.GetString("errorLoadingImage"));
        }



        #endregion
        #region CUSTOM METHODS



        /// <summary>
        ///     Loads image 
        /// </summary>
        /// <param name="filepath">path of the image to be loaded</param>
        private void loadImage(string filepath)
        {
            if (filepath != null)
            {
                progressRing.Visibility = Visibility.Visible;
                showMessage(rl.GetString("textValidatingInternetConnection"));
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    showMessage(rl.GetString("textLoadingImage"));
                    imageHeader.Source = Utils.getBitmapImageFromPath(filepath);

                }
                else
                {
                    showMessage(rl.GetString("errorNoInternetConnection"));
                    progressRing.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                progressRing.Visibility = Visibility.Collapsed;
                showMessage(rl.GetString("errorNoData"));
            }
        }



        /// <summary>
        ///     Shows appropriate message
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        private void showMessage(string message)
        {
            if (LoadingPanel.Visibility != Visibility.Visible)
                LoadingPanel.Visibility = Visibility.Visible;

            if (textBlockMessage.Visibility != Visibility.Visible)
                textBlockMessage.Visibility = Visibility.Visible;

            if (message != null)
                textBlockMessage.Text = message;
        }



        /// <summary>
        ///     If there's any text to be set, set it, 
        ///     otherwise use localised string for no Data
        /// </summary>
        /// <param name="textBlock">TextBlock to hold the text</param>
        /// <param name="text">text to be set</param>
        private void assertTextBlock(TextBlock textBlock, string text)
        {
            if (text != null)
            {
                textBlock.Text = Windows.Data.Html.HtmlUtilities.ConvertToText(text);
            }
            else
            {
                textBlock.Text = rl.GetString("errorNoData");
            }

        }



        #endregion

        private void Scroll_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var verticalOffset = ScrollViewerContent.VerticalOffset;
            double newHeight = originalHeight - verticalOffset;

#if DEBUG
            Debug.WriteLine("[ContentDetailsUserControl][Scroll_ViewChanging] Vertical Offset: {0}, newHeight: {1}",
                verticalOffset, newHeight);
#endif
        }
    }
}
