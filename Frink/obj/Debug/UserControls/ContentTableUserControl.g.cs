﻿

#pragma checksum "D:\Projekti\WindowsPhone\project_f\Frink\UserControls\ContentTableUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E42CD9549FF333CB44B3ED79213F566B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Frink.UserControls
{
    partial class ContentTableUserControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 23 "..\..\UserControls\ContentTableUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageOpened += this.imageHeader_ImageOpened;
                 #line default
                 #line hidden
                #line 24 "..\..\UserControls\ContentTableUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageFailed += this.imageHeader_ImageFailed;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 58 "..\..\UserControls\ContentTableUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ListViewContent_ItemClick;
                 #line default
                 #line hidden
                #line 59 "..\..\UserControls\ContentTableUserControl.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.ListViewContent_Loaded;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

