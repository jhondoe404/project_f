using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using HockeyApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Frink
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        #region CLASS PARAMETERS



        IBackgroundTaskRegistration taskRegistration;

        /// <summary>
        ///     Returns if there are any registered tasks.
        /// </summary>
        public bool TaskIsRegistered
        {
            get
            {
                IReadOnlyDictionary<Guid, IBackgroundTaskRegistration> allTasks = BackgroundTaskRegistration.AllTasks;
                return (allTasks.Count > 0);
            }
        }

        private TransitionCollection transitions;
        private string eTagTheme;



        #endregion
        #region CLASS CONSTRUCT



        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            HardwareButtons.BackPressed += NavigationDelegate.HardwareButtons_BackPressed;
            HockeyClient.Current.Configure("2c07ec5ac57e4d4f88cb1699614005bf");
        }



        #endregion
        #region APP LIFECYCLE METHODS



        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        async protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

#if DEBUG
                Debug.WriteLine("[App.xaml.cs] loading file ");
#endif
                if (await FileHelper.ValidateFile(ApplicationData.Current.TemporaryFolder, ConstantsHelper.LOCAL_FILE_APPLICATION_THEME) == false)
                {
#if DEBUG
                    Debug.WriteLine("[App.xaml.cs] file is null");
#endif

                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                } 
                else
                {                    
#if DEBUG
                        Debug.WriteLine("[App.xaml.cs] file is NOT null ");
#endif
                        string[] apptheme = await FileHelper.readHttpFromFile(ConstantsHelper.LOCAL_FILE_APPLICATION_THEME, ApplicationData.Current.TemporaryFolder);
                        DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(apptheme[0]);
                        ThemeDelegate.refreshTheme();

                        // When the navigation stack isn't restored navigate to the first page,
                        // configuring the new page by passing required information as a navigation
                        // parameter
                        if (DataHelper.Instance._themeModel.menu.type.Equals(ConstantsHelper.NAVIGATION_TYPE_DRAWER))
                        {
                            if (!rootFrame.Navigate(typeof(NavigationDrawerPage), e.Arguments))
                            {
                                throw new Exception("Failed to create initial navigation page");
                            }
                        }
                        else
                        {
                            if (!rootFrame.Navigate(typeof(NavigationListPage), e.Arguments))
                            {
                                throw new Exception("Failed to create initial navigation page");
                            }
                        }
                        
                    }
                }            
            
            // Ensure the current window is active
            Window.Current.Activate();
            Window.Current.VisibilityChanged += Current_VisibilityChanged;
            await HockeyClient.Current.SendCrashesAsync();
#if WINDOWS_PHONE_APP
            await HockeyClient.Current.CheckForAppUpdateAsync();
#endif
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }


        /// <summary>
        /// Invoked when application execution is being resumed.  Application state is restored
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the resumed request.</param>
        /// <param name="e">Details about the resume request.</param>
        private void OnResuming(object sender, object e)
        {
            if (this.TaskIsRegistered)
            {
                this.GetTask();
            }
            else
            {
                RegisterTask(ConstantsHelper.BACKGROUND_TASK_RESTFUL_API);
            }
        }


        /// <summary>
        /// Inovked when the application visibility has changed. This is used because the 
        /// onResuming and on Suspending event handlers aren't firing properly.
        /// </summary>
        /// <param name="sender">The source of the event invoker.</param>
        /// <param name="e">Details about the visibility state.</param>
        private void Current_VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            if (e.Visible)
            {
                if (this.TaskIsRegistered)
                {
                    this.GetTask();
                }
                else
                {
                    RegisterTask(ConstantsHelper.BACKGROUND_TASK_RESTFUL_API);
                }
            }
            else
            {
                this.UnregisterTask();
            }
        }



        #endregion
        #region BACKGROUND TASKS



        /// <summary>
        ///     Adds a task to the background queue with appropriate System Trigger.
        /// </summary>
        /// <param name="name">Name of the background task in the queue</param>
        public void RegisterTask(string name)
        {
            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = name;
            SystemTrigger trigger = new SystemTrigger(SystemTriggerType.InternetAvailable, false);
            taskBuilder.SetTrigger(trigger);
            taskBuilder.TaskEntryPoint = typeof(BackgroundRest.RestfulAPI).FullName;
            taskBuilder.Register();
            this.GetTask();
        }


        /// <summary>
        ///     Removes the task from the registered queue.
        /// </summary>
        public void UnregisterTask()
        {
            if (taskRegistration != null)
            {
                this.taskRegistration.Completed -= OnCompleted;
                this.taskRegistration.Progress -= OnProgress;
                this.taskRegistration.Unregister(false);
                this.taskRegistration = null;
            }            
        }


        /// <summary>
        ///     Gets the first registered task and assigns event handlers to it.
        /// </summary>
        public void GetTask()
        {
            this.taskRegistration = BackgroundTaskRegistration.AllTasks.Values.First();
            this.taskRegistration.Completed += OnCompleted;
            this.taskRegistration.Progress += OnProgress;
        }

        /// <summary>
        ///     Event handler that fires off every time a background task does something
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async private void OnProgress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
#if DEBUG
                    Debug.WriteLine("[BackgroundDelegate][OnProgress] {0}", args.Progress);
#endif
                    if (args.Progress == 7)
                    {
                        bool fileExists = await FileHelper.ValidateFile(ApplicationData.Current.TemporaryFolder, ConstantsHelper.LOCAL_FILE_APPLICATION_THEME);
                        if (fileExists)
                        {
                            string[] local = await FileHelper.readHttpFromFile(ConstantsHelper.LOCAL_FILE_APPLICATION_THEME, ApplicationData.Current.TemporaryFolder);
                            if (local.Length > 1 && local[1] != null && this.eTagTheme != local[1])
                            {
                                this.eTagTheme = local[1];
                                string navigationType = DataHelper.Instance._themeModel.menu.type;
                                DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(local[0]);
                                ThemeDelegate.refreshTheme();

                                // Check if navigation type has changed
                                if (!navigationType.Equals(DataHelper.Instance._themeModel.menu.type))
                                {
                                    Frame rootFrame = Window.Current.Content as Frame;

                                    if (DataHelper.Instance._themeModel.menu.type.Equals(ConstantsHelper.NAVIGATION_TYPE_DRAWER))
                                    {                                        
                                        rootFrame.Navigate(typeof(NavigationDrawerPage));
                                    }
                                    else
                                    {
                                        rootFrame.Navigate(typeof(NavigationListPage));
                                    }

                                    rootFrame.BackStack.Clear();
                                }
                            }
                        }
                        else
                        {
#if DEBUG
                            Debug.WriteLine("[Application][OnProgress] File doesn't exist");
#endif
                        }
                    }

                });
        }


        /// <summary>
        ///     Fires off when the background task is finished with the task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async private void OnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    // TODO: end task logic
                    Debug.WriteLine("[Application][OnCompleted]");
                });
        }



        #endregion
    }
}