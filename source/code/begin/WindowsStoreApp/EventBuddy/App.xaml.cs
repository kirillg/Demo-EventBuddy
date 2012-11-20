﻿using Microsoft.Live;
using EventBuddy.Common;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EventBuddy
{
    sealed partial class App : Application
    {
        //TODO: add mobile service client


        #region

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            RequestedTheme = ApplicationTheme.Light;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    ///TODO:
                    //try
                    //{
                    //    await SuspensionManager.RestoreAsync();
                    //}
                    //catch (SuspensionManagerException)
                    //{
                    //    //Something went wrong restoring state.
                    //    //Assume there is no state and continue
                    //}
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(EventsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
    
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO:
            //var deferral = e.SuspendingOperation.GetDeferral();
        
            //await SuspensionManager.SaveAsync();
            //deferral.Complete();
        }

        public static LiveConnectSession LiveSession;

        #endregion
    }
}
