using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentGridPage : Page
    {
        #region CLASS CONSTRUCT



        public ContentGridPage()
        {
            this.InitializeComponent();

            if (DataHelper.Instance.themeColor_1 != null)
                textBlockDescription.Foreground = DataHelper.Instance.themeColor_1;
        }



        #endregion
        #region LIFECYCLE METHODS



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentGridPage][OnNavigatedTo]");
#endif
            if (e == null)
                return;

            string myPages = "";
            foreach (PageStackEntry page in Frame.BackStack)
            {
                myPages += page.SourcePageType.ToString() + "\n";
            }
#if DEBUG
            Debug.WriteLine("[ContentGridPage][OnNavigatedTo] pages {0}", myPages);
#endif

            MenuItemModel item = e.Parameter as MenuItemModel;
            var bs = Frame.BackStack.Where(b => b.SourcePageType.Name == "NavigationListPage").FirstOrDefault();
            Frame.BackStack.Add(bs);
#if DEBUG
            Debug.WriteLine("[ContentGridPage][OnNavigatedTo] {0}", item.type);
#endif

            await RestService.getEntry(item.source);
            if (DataHelper.Instance._contentItemModel != null && DataHelper.Instance._contentItemModel.Count > 0)
            {
                textBlockDescription.Text = DataHelper.Instance._contentItemModel[0].description;
                gridViewcontent.ItemsSource = DataHelper.Instance._contentItemModel[0].images;
            }
            else
            {
                MessageDialog message = new MessageDialog(new ResourceLoader().GetString("errorNoData"));
                await message.ShowAsync();
            }
        }



        #endregion
        #region EVENT HANDLERS



        async private void ListViewContent_ItemClick(object sender, ItemClickEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentGridPage][ItemClicked]");
#endif
            MessageDialog message = new MessageDialog("Click event not yet implemented");
            await message.ShowAsync();
        }


        private void gridViewcontent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = (WrapGrid)gridViewcontent.ItemsPanelRoot;
            panel.ItemWidth = panel.ItemHeight = root.ActualWidth / 4;
        }



        #endregion  
    }
}
