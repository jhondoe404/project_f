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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentDetailsPage : Page
    {
        #region CLASS PARAMETERS



        private double originalHeight;
        ResourceLoader rl;



        #endregion
        #region CLASS CONSTRUCT



        public ContentDetailsPage()
        {
            this.InitializeComponent();
        }



        #endregion
        #region LIFECYCLE METHODS



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentDetailsPage][OnNavigatedTo]");
#endif
            if (e == null)
                return;

            ContentItemModel item = e.Parameter as ContentItemModel;
            if (item == null)
                return;

            if (rl == null)
                rl = new ResourceLoader();
#if DEBUG
            Debug.WriteLine("[ContentTablePage][OnNavigatedTo] {0}", item.author);
#endif
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
            Debug.WriteLine("[ContentDetailsPage][Scroll_ViewChanging] Vertical Offset: {0}, newHeight: {1}",
                verticalOffset, newHeight);
#endif
        }
    }
}
