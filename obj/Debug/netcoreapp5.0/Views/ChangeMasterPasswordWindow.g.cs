﻿#pragma checksum "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C51A3CE6B947298BC0E15732B246B80F826D07D3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// ChangeMasterPasswordWindow
    /// </summary>
    public partial class ChangeMasterPasswordWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal OlibKey.Views.ChangeMasterPasswordWindow mainWindow;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform ScaleWindow;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TxtOldPassword;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtOldPasswordCollapsed;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CbOldHide;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TxtPassword;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtPasswordCollapsed;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CbHide;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar PbHard;
        
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
            System.Uri resourceLocater = new System.Uri("/OlibKey;V2.1.0.0;component/views/changemasterpasswordwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
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
            this.mainWindow = ((OlibKey.Views.ChangeMasterPasswordWindow)(target));
            return;
            case 2:
            this.ScaleWindow = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 3:
            
            #line 55 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 58 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 71 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            ((System.Windows.Media.Animation.Storyboard)(target)).Completed += new System.EventHandler(this.Timeline_OnCompleted);
            
            #line default
            #line hidden
            return;
            case 6:
            this.TxtOldPassword = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 7:
            this.TxtOldPasswordCollapsed = ((System.Windows.Controls.TextBox)(target));
            
            #line 90 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.TxtOldPasswordCollapsed.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtOldPasswordCollapsed_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CbOldHide = ((System.Windows.Controls.CheckBox)(target));
            
            #line 91 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.CbOldHide.Checked += new System.Windows.RoutedEventHandler(this.OldCollapsedPassword);
            
            #line default
            #line hidden
            
            #line 91 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.CbOldHide.Unchecked += new System.Windows.RoutedEventHandler(this.OldCollapsedPassword);
            
            #line default
            #line hidden
            return;
            case 9:
            this.TxtPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 96 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.TxtPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.txtPassword_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.TxtPasswordCollapsed = ((System.Windows.Controls.TextBox)(target));
            
            #line 97 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.TxtPasswordCollapsed.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtPasswordCollapsed_TextChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.CbHide = ((System.Windows.Controls.CheckBox)(target));
            
            #line 98 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.CbHide.Checked += new System.Windows.RoutedEventHandler(this.CollapsedPassword);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.CbHide.Unchecked += new System.Windows.RoutedEventHandler(this.CollapsedPassword);
            
            #line default
            #line hidden
            return;
            case 12:
            this.PbHard = ((System.Windows.Controls.ProgressBar)(target));
            
            #line 101 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            this.PbHard.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.pbHard_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 109 "..\..\..\..\Views\ChangeMasterPasswordWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

