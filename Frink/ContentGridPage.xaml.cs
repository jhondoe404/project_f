using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Resources;
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
    public sealed partial class ContentGridPage : Page
    {
        #region CLASS PARAMETERS



        ResourceLoader rl;



        #endregion
        #region CLASS PARAMETERS



        string source;



        #endregion
        #region CLASS CONSTRUCT



        public ContentGridPage()
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
            Debug.WriteLine("[ContentGridPage][OnNavigatedTo]");
#endif
            if (e == null)
                return;

            if (rl == null)
                rl = new ResourceLoader();

            MenuItemModel item = e.Parameter as MenuItemModel;            
#if DEBUG
            Debug.WriteLine("[ContentGridPage][OnNavigatedTo] {0}", item.type);
#endif
            source = item.source;            
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
            showMessage(rl.GetString("errorLoadingImage"));
        }



        #endregion
        #region GRID VIEW



        async private void gridViewcontent_Loaded(object sender, RoutedEventArgs e)
        {
            loadImage("http://i585.photobucket.com/albums/ss296/pusangnegro/cutechicks.jpg");
            if (gridViewcontent.ItemsSource == null)
            {
                if (DataHelper.Instance._contentItemModel == null || !source.Equals(DataHelper.Instance._source))
                {
                    DataHelper.Instance._source = source;
                    await RestService.getEntry(source);
                }

                if (DataHelper.Instance._contentItemModel != null && DataHelper.Instance._contentItemModel.Count > 0)
                {
                    textBlockDescription.Text = DataHelper.Instance._contentItemModel[0].description;
                    gridViewcontent.ItemsSource = DataHelper.Instance._contentItemModel[0].images;
                }
                else
                {
                    MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoData"));
                    await message.ShowAsync();
                }
            }
        }



        private void ListViewContent_ItemClick(object sender, ItemClickEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentGridPage][ItemClicked]");
#endif
            if (sender == null)
                return;

            ImageModel item = (ImageModel)e.ClickedItem;
            if (item == null)
                return;

            ContentItemModel newContentItemModel = new ContentItemModel();
            newContentItemModel.type = ContentItemModel.CONTENT_ITEM_GRID;
            newContentItemModel.images = new System.Collections.ObjectModel.ObservableCollection<ImageModel>();
            newContentItemModel.images.Add(item);

            Frame.Navigate(typeof(ContentDetailsPage), newContentItemModel);
        }



        private void WrapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var wg = sender as WrapGrid;
            wg.ItemWidth = wg.ItemHeight = (int)gridViewcontent.ActualWidth / 3;
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



        #endregion
    }
}
