﻿

#pragma checksum "D:\Projekti\WindowsPhone\project_f\Frink\NavigationListPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D35FC02215F140D5B7C0DA2716AB65A9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Frink
{
    partial class NavigationListPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 41 "..\..\NavigationListPage.xaml"
                ((global::ExtendedListView.ExtendedListView)(target)).PullToRefreshRequested += this.ListViewNavigationMain_PullToRefreshRequested;
                 #line default
                 #line hidden
                #line 42 "..\..\NavigationListPage.xaml"
                ((global::ExtendedListView.ExtendedListView)(target)).ItemClick += this.ListViewNavigationMain_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 23 "..\..\NavigationListPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageOpened += this.imageHeader_ImageOpened;
                 #line default
                 #line hidden
                #line 24 "..\..\NavigationListPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageFailed += this.imageHeader_ImageFailed;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


