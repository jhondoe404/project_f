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
        }

        private void imageHeader_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            showMessage(textBlockErrorLoading);
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
                showMessage(textBlockValidatingConnection);
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    showMessage(textBlockLoadingImage);
                    imageHeader.Source = Utils.getBitmapImageFromPath(filepath);

                }
                else
                {
                    showMessage(textBlockErrorNoConnection);
                    progressRing.Visibility = Visibility.Collapsed;
                }                
            }
            else
            {
                showMessage(textBlockErrorNoData);
            }            
        }



        /// <summary>
        ///     Shows appropriate message and hides the rest
        /// </summary>
        /// <param name="textBlock">TextBlock to be shown</param>
        private void showMessage(TextBlock textBlock)
        {
            textBlockErrorNoConnection.Visibility = Visibility.Collapsed;
            textBlockErrorLoading.Visibility = Visibility.Collapsed;
            textBlockErrorNoData.Visibility = Visibility.Collapsed;
            textBlockLoadingImage.Visibility = Visibility.Collapsed;
            textBlockValidatingConnection.Visibility = Visibility.Collapsed;

            LoadingPanel.Visibility = Visibility.Visible;
            textBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
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
                textBlock.Text = text;
            }
            else
            {
                textBlock.Text = new ResourceLoader().GetString("errorNoData");
            }

        }



        #endregion        
    }
}
