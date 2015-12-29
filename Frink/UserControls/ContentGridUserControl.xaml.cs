using Frink.Helpers;
using Frink.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ContentGridUserControl : UserControl
    {
        #region CLASS PARAMETERS



        private ResourceLoader rl;
        private ContentItemModel _contentItemModel; 



        #endregion
        #region CLASS DELEGATES



        public delegate void ChildControlDelegate(ContentItemModel item);
        public event ChildControlDelegate GetItemDetails;



        #endregion
        #region CLASS CONSTRUCT



        public ContentGridUserControl()
        {
            this.InitializeComponent();
            init(null);
        }

        public ContentGridUserControl(ContentItemModel item)
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
                _contentItemModel = item;                  
        }



        #endregion
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
        #region GRID METHODS



        private void gridViewcontent_Loaded(object sender, RoutedEventArgs e)
        {
            loadImage("http://i585.photobucket.com/albums/ss296/pusangnegro/cutechicks.jpg");

            if (_contentItemModel != null)
            {
                textBlockDescription.Text = _contentItemModel.description;
                gridViewcontent.ItemsSource = _contentItemModel.images;
            }      
        }

        private void ListViewContent_ItemClick(object sender, ItemClickEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentGridUserControl][ItemClicked]");
#endif
            if (sender == null)
                return;

            ImageModel item = (ImageModel)e.ClickedItem;
            if (item == null)
                return;

            ContentItemModel newContentItemModel = new ContentItemModel();
            newContentItemModel.type = ContentItemModel.CONTENT_ITEM_GRID;
            newContentItemModel.images = new ObservableCollection<ImageModel>();
            newContentItemModel.images.Add(item);

            if (GetItemDetails != null)
                GetItemDetails(newContentItemModel);            
        }


        private void WrapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var wg = sender as WrapGrid;
            wg.ItemWidth = wg.ItemHeight = (int)gridViewcontent.ActualWidth / 3;
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



        #endregion       
    }
}
