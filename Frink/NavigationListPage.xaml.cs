﻿using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationListPage : Page
    {
        #region CLASS CONSTRUCT



        public NavigationListPage()
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
            if (e == null)
                return;
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
        #region SCROLL VIEWVER



        private void ListViewNavigationMain_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataHelper.Instance._themeModel != null)
            {
                loadImage(DataHelper.Instance._themeModel.menu.image);
                if (ListViewNavigationMain.ItemsSource == null)
                {
                    ListViewNavigationMain.ItemsSource = DataHelper.Instance._themeModel.menu.items;
                }            
            }
        }



        private async Task ListViewNavigationMain_PullToRefreshRequested(object sender, EventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[NavigationListPage][ListViewNavigationMain_PullToRefreshRequested] Refreshing the content");
#endif
            await Task.Delay(1000);

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                await RestService.getMenu();
                if (DataHelper.Instance._themeModel == null)
                {
                    MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorLoadingData"));
                    await message.ShowAsync();
                }
            }
            else
            {
                MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoInternetConnection"));
                await message.ShowAsync();
            }
        }


        async private void ListViewNavigationMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItemModel item = (MenuItemModel)e.ClickedItem;
            if (item == null || sender == null)
                return;

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoInternetConnection"));
                await message.ShowAsync();
                return;
            }
#if DEBUG
            Debug.WriteLine("[NavigationListPage][Itemclick] {0}", item.name);
#endif
            if (item.type.Equals(ConstantsHelper.NAVIGATION_TYPE_TABLE))
            {
                Frame.Navigate(typeof(ContentTablePage), item);
            }
            else if (item.type.Equals(ConstantsHelper.NAVIGATION_TYPE_GRID))
            {
                Frame.Navigate(typeof(ContentGridPage), item);
            }
            else
            {
                MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNavigationNotSupported"));
                await message.ShowAsync();
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
