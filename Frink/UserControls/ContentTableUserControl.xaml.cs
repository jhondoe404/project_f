using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
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
using Windows.UI.Popups;
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
    public partial class ContentTableUserControl : UserControl
    {
        #region CLASS PARAMETERS



        private ResourceLoader                          rl;
        private ObservableCollection<ContentItemModel>  _contentItemModel;        



        #endregion
        #region CLASS DELEGATES



        public delegate void ChildControlDelegate(ContentItemModel item);
        public event ChildControlDelegate GetItemDetails;



        #endregion
        #region CLASS CONSTRUCT



        public ContentTableUserControl()
        {
            this.InitializeComponent();
            if (rl == null)
                rl = new ResourceLoader();
        }


        public ContentTableUserControl(ObservableCollection<ContentItemModel> contentItemModel)        
        {
            this.InitializeComponent();
            if (rl == null)
                rl = new ResourceLoader();
            this._contentItemModel = contentItemModel;
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
        #region LIST VIEW


        private void ListViewContent_Loaded(object sender, RoutedEventArgs e)
        {
            loadImage("http://i585.photobucket.com/albums/ss296/pusangnegro/cutechicks.jpg");
            
            if (ListViewContent.FontFamily == null || ListViewContent.FontFamily != DataHelper.Instance._body)
            {
                ListViewContent.FontFamily = DataHelper.Instance._body;
            }

            ListViewContent.ItemsSource = _contentItemModel;            

            DateTime thisDay = DateTime.Today;
            
            contentTitle.Visibility = Visibility.Collapsed;
            contentDate.FontFamily = DataHelper.Instance._body;
            contentDate.Text = thisDay.ToString("MMMM dd, yyyy");
        }



        private void ListViewContent_ItemClick(object sender, ItemClickEventArgs e)
        {
            
#if DEBUG
            Debug.WriteLine("[ContentTableUserControl][ItemClicked]");
#endif
            if (sender == null)
                return;

            ContentItemModel item = (ContentItemModel)e.ClickedItem;
            if (item == null)
                return;

            item.type = ContentItemModel.CONTENT_ITEM_TABLE;
            if (GetItemDetails != null)
            {
                GetItemDetails(item);
            }             
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
