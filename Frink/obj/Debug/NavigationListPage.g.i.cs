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
    partial class NavigationListPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::ExtendedListView.ExtendedListView ListViewNavigationMain; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image imageHeader; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel LoadingPanel; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing progressRing; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock textBlockLoadingImage; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock textBlockValidatingConnection; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock textBlockErrorLoading; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock textBlockErrorNoConnection; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock textBlockErrorNoData; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///NavigationListPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ListViewNavigationMain = (global::ExtendedListView.ExtendedListView)this.FindName("ListViewNavigationMain");
            imageHeader = (global::Windows.UI.Xaml.Controls.Image)this.FindName("imageHeader");
            LoadingPanel = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("LoadingPanel");
            progressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("progressRing");
            textBlockLoadingImage = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("textBlockLoadingImage");
            textBlockValidatingConnection = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("textBlockValidatingConnection");
            textBlockErrorLoading = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("textBlockErrorLoading");
            textBlockErrorNoConnection = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("textBlockErrorNoConnection");
            textBlockErrorNoData = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("textBlockErrorNoData");
        }
    }
}



