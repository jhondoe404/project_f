using Frink.Helpers;
using Frink.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class NavigationListPage : Page
    {
        public NavigationListPage()
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
            if (DataHelper.Instance._themeModel != null)
            {
                ListViewNavigationMain.ItemsSource = DataHelper.Instance._themeModel.menu.items;
                imageHeader.ImageSource = DataHelper.Instance._themeModel.menu.image;
            }

            //startTask();
            
        }

      

        #region CUSTOM METHODS



        async private void startTask()
        {
            string myTaskName = "FirstTask";

            // check if task is already registered
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
                if (cur.Value.Name == myTaskName)
                {
                    await(new MessageDialog("Task already registered")).ShowAsync();
                    return;
                }

            // Windows Phone app must call this to use trigger types (see MSDN)
            await BackgroundExecutionManager.RequestAccessAsync();

            // register a new task
            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder { Name = "First Task", TaskEntryPoint = "BackgroundRest.RestfulAPI" };
            taskBuilder.SetTrigger(new TimeTrigger(1, false));
            BackgroundTaskRegistration myFirstTask = taskBuilder.Register();

            await(new MessageDialog("Task registered")).ShowAsync();
        }


        public static ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        public static ScrollBar GetScrollBar(DependencyObject depObj)
        {
            if (depObj is ScrollBar) return depObj as ScrollBar;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollBar(child);
                if (result != null) return result;
            }
            return null;
        }


        public static ListView GetListView(DependencyObject depObj)
        {
            if (depObj is ListView) return depObj as ListView;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetListView(child);
                if (result != null) return result;
            }
            return null;
        }



        #endregion
        #region SCROLL VIEWVER 


        
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



        #endregion
    }
}
