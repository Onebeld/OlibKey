﻿#pragma checksum "..\..\..\..\..\Views\SearchWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6C2DDF9BE58246A81FEAA3C14A2351DA7C1632A7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using OlibKey.ModelViews;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace OlibKey.Views {
    
    
    /// <summary>
    /// SearchWindow
    /// </summary>
    public partial class SearchWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal OlibKey.Views.SearchWindow mainWindow;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal OlibKey.ModelViews.SearchViewModel SearchModel;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform ScaleWindow;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem FullMenu;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rAll;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rLogin;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rBankCard;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rPassport;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rReminder;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\..\..\..\Views\SearchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbFolders;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/OlibKey;V2.1.0.0;component/views/searchwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\SearchWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.mainWindow = ((OlibKey.Views.SearchWindow)(target));
            
            #line 9 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.mainWindow.Loaded += new System.Windows.RoutedEventHandler(this.mainWindow_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SearchModel = ((OlibKey.ModelViews.SearchViewModel)(target));
            return;
            case 3:
            this.ScaleWindow = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 4:
            
            #line 74 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.OnMouseMove);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.OnMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FullMenu = ((System.Windows.Controls.MenuItem)(target));
            
            #line 78 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.FullMenu.Click += new System.Windows.RoutedEventHandler(this.Full);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 84 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click_1);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 92 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Full);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 99 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Media.Animation.Storyboard)(target)).Completed += new System.EventHandler(this.Timeline_OnCompleted);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 120 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 123 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.ListBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.rAll = ((System.Windows.Controls.RadioButton)(target));
            
            #line 133 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.rAll.Checked += new System.Windows.RoutedEventHandler(this.rLogin_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.rLogin = ((System.Windows.Controls.RadioButton)(target));
            
            #line 145 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.rLogin.Checked += new System.Windows.RoutedEventHandler(this.rLogin_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.rBankCard = ((System.Windows.Controls.RadioButton)(target));
            
            #line 155 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.rBankCard.Checked += new System.Windows.RoutedEventHandler(this.rLogin_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.rPassport = ((System.Windows.Controls.RadioButton)(target));
            
            #line 165 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.rPassport.Checked += new System.Windows.RoutedEventHandler(this.rLogin_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.rReminder = ((System.Windows.Controls.RadioButton)(target));
            
            #line 175 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.rReminder.Checked += new System.Windows.RoutedEventHandler(this.rLogin_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.lbFolders = ((System.Windows.Controls.ListBox)(target));
            
            #line 187 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.lbFolders.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.lbFolders_MouseDown);
            
            #line default
            #line hidden
            
            #line 187 "..\..\..\..\..\Views\SearchWindow.xaml"
            this.lbFolders.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbFolders_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 190 "..\..\..\..\..\Views\SearchWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

