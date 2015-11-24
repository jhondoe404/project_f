using Frink.Helpers;
using Frink.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Frink.UserControls
{
    public sealed partial class ImageLoaderUserControl : UserControl
    {
        #region CLASS PARAMETERS



        private AsyncTask _loadImage;
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register
           (
               "ImageSource",
               typeof(string),
               typeof(MainNavigationUserControl),
               new PropertyMetadata(null, UpdateImageSource)
           );



        #endregion
        #region CLASS CONSTRUCT



        public ImageLoaderUserControl()
        {
            this.InitializeComponent();
        }



        #endregion
        #region CALLBACKS



        private static void UpdateImageSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null || e == null || e.OldValue == e.NewValue)
                return;

            ImageLoaderUserControl userControl = (ImageLoaderUserControl)d;

            if (userControl == null)
                return;

            userControl.loadImage((string)e.NewValue);
        }



        #endregion
        #region IMAGE LOADING METHODS



        private void imageHeader_ImageOpened(object sender, RoutedEventArgs e)
        {
            LoadingPanel.Visibility = Visibility.Collapsed;
        }

        private void imageHeader_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            showMessage(textBlockErrorLoading);
        }



        #endregion
        #region CUSTOM METHODS



        /// <summary>
        ///  Loads image from a specified path
        /// </summary>
        /// <param name="imagePath">path to the image to be loaded</param>
        private async void loadImage(string imagePath)
        {
#if DEBUG
            Debug.WriteLine("[ImageLoaderUserControl][loadImage] loading image");
#endif
            showMessage(textBlockLoadingImage);
            progressRing.Visibility = Visibility.Visible;
            string filePath = imagePath;

            if (imagePath.Contains("http://") || imagePath.Contains("https://"))
            {
#if DEBUG
                Debug.WriteLine("[ImageLoaderUserControl][loadImage] image is from url {0}", imagePath);
#endif
                filePath = null;

                string fileName = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
                bool fileExists = await FileHelper.ValidateFile(ApplicationData.Current.TemporaryFolder, fileName);

                if (!fileExists)
                {
#if DEBUG
                    Debug.WriteLine("[ImageLoaderUserControl][loadImage] file does not exists");
#endif
                    showMessage(textBlockValidatingConnection);
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
#if DEBUG
                        Debug.WriteLine("[ImageLoaderUserControl][loadImage] internet connection validation passed");
#endif
                        showMessage(textBlockLoadingImage);

                        if (_loadImage == null)
                            _loadImage = new AsyncTask();

                        _loadImage.setUrl(imagePath);
                        var result = await _loadImage.execute();
#if DEBUG
                        Debug.WriteLine("[ImageLoaderUserControl][loadImage] writing data to a local file");
#endif
                        await FileHelper.WriteToFile(result, fileName, ApplicationData.Current.TemporaryFolder);
                    }
                    else
                    {
#if DEBUG
                        Debug.WriteLine("[ImageLoaderUserControl][loadImage] no itnernet connection");
#endif
                        progressRing.Visibility = Visibility.Collapsed;
                        showMessage(textBlockErrorNoConnection);
                    }
                }


                try
                {
                    var file = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);
                    filePath = file.Path;
#if DEBUG
                    Debug.WriteLine("[ImageLoaderUserControl][loadImage] loading image from a local file {0}", filePath);
#endif 
                }
                catch (FileNotFoundException e)
                {
#if DEBUG
                    Debug.WriteLine("[ImageLoaderUserControl][loadImage] file not found {0}", e.Message);
#endif 
                }
            }

            if (filePath != null)
            {
                BitmapImage bmi = new BitmapImage();
                Uri myUri = new Uri(filePath, UriKind.Absolute);
                bmi.CreateOptions = BitmapCreateOptions.None;
                bmi.UriSource = myUri;
                imageHeader.Source = bmi;
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
            textBlockLoadingImage.Visibility = Visibility.Collapsed;
            textBlockValidatingConnection.Visibility = Visibility.Collapsed;

            LoadingPanel.Visibility = Visibility.Visible;
            textBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }



        #endregion
    }
}
