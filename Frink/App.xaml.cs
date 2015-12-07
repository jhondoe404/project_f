﻿using HockeyApp;
using Frink.Helpers;
using Frink.Models;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Frink.Delegates;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Frink
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

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
                if (await FileHelper.ValidateFile(ApplicationData.Current.LocalFolder, ConstantsHelper.LOCAL_FILE_APPLICATION_THEME) == false)
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
                        String[] apptheme = await FileHelper.readHttpFromFile(ConstantsHelper.LOCAL_FILE_APPLICATION_THEME);
                        DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(apptheme[0]);

                        // When the navigation stack isn't restored navigate to the first page,
                        // configuring the new page by passing required information as a navigation
                        // parameter
                        if (!rootFrame.Navigate(typeof(NavigationListPage), e.Arguments))
                        {
                            throw new Exception("Failed to create initial navigation page");
                        }
                    }
                }            
            
            // Ensure the current window is active
            Window.Current.Activate();
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

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}