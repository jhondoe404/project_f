﻿

#pragma checksum "D:\Projekti\WindowsPhone\project_f\Frink\UserControls\ContentDetailsUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0412B7E47811B6157E540973F99FEFD7"
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
    partial class ContentDetailsUserControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 15 "..\..\UserControls\ContentDetailsUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.ScrollViewer)(target)).ViewChanging += this.Scroll_ViewChanging;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 22 "..\..\UserControls\ContentDetailsUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageOpened += this.imageHeader_ImageOpened;
                 #line default
                 #line hidden
                #line 23 "..\..\UserControls\ContentDetailsUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Image)(target)).ImageFailed += this.imageHeader_ImageFailed;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


