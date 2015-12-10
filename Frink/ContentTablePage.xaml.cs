using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
using System;
using System.Diagnostics;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentTablePage : Page
    {
        #region CLASS CONSTRUCT



        public ContentTablePage()
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
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentTablePage][OnNavigatedTo]");
#endif
            if (e == null)
                return;
            
            MenuItemModel item = e.Parameter as MenuItemModel;
            //var bs = Frame.BackStack.Where(b => b.SourcePageType.Name == "NavigationListPage").FirstOrDefault();
            //Frame.BackStack.Add(bs);
#if DEBUG
            Debug.WriteLine("[ContentTablePage][OnNavigatedTo] {0}", item.type);
#endif            
            await RestService.getEntry(item.source);
            ListViewContent.ItemsSource = DataHelper.Instance._contentItemModel;
            loadImage("http://i585.photobucket.com/albums/ss296/pusangnegro/cutechicks.jpg");
        }



        #endregion
        #region EVENT HANDLERS
        #region IMAGE LOADER



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
        #region LIST VIEW



        private void ListViewContent_ItemClick(object sender, ItemClickEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentTablePage][ItemClicked]");
#endif
            if (sender == null)
                return;

            ContentItemModel item = (ContentItemModel)e.ClickedItem;
            if (item == null)
                return;

            item.type = ContentItemModel.CONTENT_ITEM_TABLE;
            Frame.Navigate(typeof(ContentDetailsPage), item);
        }



        #endregion
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



        #endregion
    }
}
