﻿

#pragma checksum "D:\Projekti\WindowsPhone\project_f\Frink\ContentGridPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4649D261C06F4FAAC8B9F53FD852B681"
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
    partial class ContentGridPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 54 "..\..\ContentGridPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ListViewContent_ItemClick;
                 #line default
                 #line hidden
                #line 55 "..\..\ContentGridPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.gridViewcontent_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 86 "..\..\ContentGridPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.WrapGrid_Loaded;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 25 "..\..\ContentGridPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageOpened += this.imageHeader_ImageOpened;
                 #line default
                 #line hidden
                #line 26 "..\..\ContentGridPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageFailed += this.imageHeader_ImageFailed;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


