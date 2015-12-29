using Frink.Helpers;
using Frink.Rest;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region LIFECYCLE METHODS



        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            loadApplicationData();
        }



        #endregion
        #region EVENT HANDLERS



        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[MainPage][buttonRefresh_Click] clicked");
#endif
            buttonRefresh.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            progressRing.Visibility = Windows.UI.Xaml.Visibility.Visible;
            
            loadApplicationData();
        }



        #endregion
        #region CUSTOM METHODS


        /// <summary>
        ///     Starts internet validation and handle the HTTP request appropriately
        /// </summary>
        private async void loadApplicationData()
        {
#if DEBUG
            Debug.WriteLine("[MainPage][loadApplicationData] loading application theme");
#endif

            statusMessage.Text = new ResourceLoader().GetString("loadingApplicationTheme");
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                await RestService.getMenu();
                if (DataHelper.Instance._themeModel == null)
                {
#if DEBUG
                    Debug.WriteLine("[MainPage][loadApplicationData] there was an error loading");
#endif
                    statusMessage.Text = new ResourceLoader().GetString("errorNoInternetConnection");
                    progressRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    buttonRefresh.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {                   
#if DEBUG
                    Debug.WriteLine("[MainPage][loadApplicationData] everything was done succesfully");
#endif
                    Frame.Navigate(typeof(NavigationListPage));
                }
            }
            else
            {
#if DEBUG
                Debug.WriteLine("[MainPage][loadApplicationData] no internet connection");
#endif
                progressRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                buttonRefresh.Visibility = Windows.UI.Xaml.Visibility.Visible;
                statusMessage.Text = new ResourceLoader().GetString("errorNoInternetConnection");
            }
        }



        #endregion
    }
}
