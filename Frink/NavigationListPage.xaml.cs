using Frink.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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



        #endregion
        #region SCROLL VIEWVER 
        #endregion
    }
}
