using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using Frink.Rest;
using System;
using System.Diagnostics;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Frink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentTablePage : Page
    {
        #region CLASS CONSTRUCT



        public ContentTablePage()
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
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentTablePage][OnNavigatedTo]");
#endif
            if (e == null)
                return;
            
            MenuItemModel item = e.Parameter as MenuItemModel;
            //var bs = Frame.BackStack.Where(b => b.SourcePageType.Name == "NavigationListPage").FirstOrDefault();
            //Frame.BackStack.Add(bs);
#if DEBUG
            Debug.WriteLine("[ContentTablePage][OnNavigatedTo] {0}", item.type);
#endif            
            await RestService.getEntry(item.source);
            ListViewContent.ItemsSource = DataHelper.Instance._contentItemModel;
        }



        #endregion
        #region EVENT HANDLERS



        async private void ListViewContent_ItemClick(object sender, ItemClickEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("[ContentTablePage][ItemClicked]");
#endif
            MessageDialog message = new MessageDialog("Click event not yet implemented");
            await message.ShowAsync();
        }



        #endregion
    }
}
