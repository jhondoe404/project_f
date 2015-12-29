using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
using Frink.UserControls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationDrawerPage : Page
    {
        #region CLASS PARAMETERS



        private NavigationDelegate _NavigationDelegate;



        #endregion
        #region CLASS CONSTRUCT



        public NavigationDrawerPage()
        {
            this.InitializeComponent();

            DrawerLayout.InitializeDrawerLayout();
            _NavigationDelegate = new NavigationDelegate(DrawerLayout);
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
            HardwareButtons.BackPressed += _NavigationDelegate.BackPressedStack;
        }



        #endregion
        #region EVENT HANDLERS
        #region IMAGE LOADER



        private void imageHeader_ImageOpened(object sender, RoutedEventArgs e)
        {
            //LoadingPanel.Visibility = Visibility.Collapsed;
        }

        private void imageHeader_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //progressRing.Visibility = Visibility.Collapsed;
            //showMessage(rl.GetString("errorLoadingImage"));
        }



        #endregion
        #region DRAWER NAVIGATION



        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
                DrawerLayout.OpenDrawer();
        }


        async private void ListMenuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListMenuItems.SelectedItem != null)
            {
                DrawerLayout.CloseDrawer();

                // Validate connectivity
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoInternetConnection"));
                    await message.ShowAsync();
                    return;
                }

                // Get data
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.ProgressIndicator.ShowAsync();

                MenuItemModel item = (MenuItemModel)ListMenuItems.SelectedItem;
                if (DataHelper.Instance._contentItemModel == null || !item.source.Equals(DataHelper.Instance._source))
                {
                    DataHelper.Instance._source = item.source;
                    await RestService.getEntry(item.source);
                }

                await statusBar.ProgressIndicator.HideAsync();
                
                // Validate data and load apropriate control
                if (!(DataHelper.Instance._contentItemModel != null && DataHelper.Instance._contentItemModel.Count > 0))
                {
                    MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoData"));
                    await message.ShowAsync();                    
                }
                else
                {
                    switch (item.type)
                    {
                        case ConstantsHelper.NAVIGATION_TYPE_TABLE:
                            ContentTableUserControl uControlContentTable = new ContentTableUserControl(DataHelper.Instance._contentItemModel);
                            uControlContentTable.GetItemDetails += uControlContentTable_GetItemDetails;
                            this._NavigationDelegate.AddItemTabbed(uControlContentTable);                            
                            break;

                        case ConstantsHelper.NAVIGATION_TYPE_GRID:
                            ContentGridUserControl uControlContentGrid = new ContentGridUserControl(DataHelper.Instance._contentItemModel[0]);
                            uControlContentGrid.GetItemDetails += uControlContentTable_GetItemDetails;
                            this._NavigationDelegate.AddItemTabbed(uControlContentGrid);                            
                            break;

                        default:
                            MessageDialog messageNotSupported = new MessageDialog(new ResourceLoader().GetString("errorNavigationNotSupported"));
                            await messageNotSupported.ShowAsync();
                            break;
                    }    
                }

                ListMenuItems.SelectedItem = null;                
            }
        }

        void uControlContentTable_GetItemDetails(ContentItemModel item)
        {
            this._NavigationDelegate.AddItem(new ContentDetailsUserControl(item));
        }



        #endregion
        #region SCROLL VIEWVER



        private void ListMenuItems_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataHelper.Instance._themeModel != null)
            {
                //loadImage(DataHelper.Instance._themeModel.menu.image);
                if (ListMenuItems.FontFamily == null || ListMenuItems.FontFamily != DataHelper.Instance._body)
                {
                    ListMenuItems.FontFamily = DataHelper.Instance._body;
                }
                if (ListMenuItems.ItemsSource == null)
                {
                    ListMenuItems.ItemsSource = DataHelper.Instance._themeModel.menu.items;
                }
            }
        }


        private async Task ListViewNavigationMain_PullToRefreshRequested(object sender, EventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[NavigationListPage][ListViewNavigationMain_PullToRefreshRequested] Refreshing the content");
#endif
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                await RestService.getMenu();
                if (DataHelper.Instance._themeModel == null)
                {
                    MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorLoadingData"));
                    await message.ShowAsync();
                }

                if (ListMenuItems.FontFamily == null || ListMenuItems.FontFamily != DataHelper.Instance._body)
                {
                    ListMenuItems.FontFamily = DataHelper.Instance._body;
                }
            }
            else
            {
                MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoInternetConnection"));
                await message.ShowAsync();
            }
        }


        #endregion
        #endregion
    }
}
