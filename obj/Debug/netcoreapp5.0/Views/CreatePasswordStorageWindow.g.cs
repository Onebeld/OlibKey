﻿#pragma checksum "..\..\..\..\Views\CreatePasswordStorageWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1F9DAC8B5001BEECE6FBEDDADA01EBB9F9BA5498"
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
    /// CreatePasswordStorageWindow
    /// </summary>
    public partial class CreatePasswordStorageWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal OlibKey.Views.CreatePasswordStorageWindow mainWindow;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform ScaleWindow;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtPathSelection;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TxtPassword;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtPasswordCollapsed;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CbHide;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/OlibKey;V2.1.0.0;component/views/createpasswordstoragewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
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
            this.mainWindow = ((OlibKey.Views.CreatePasswordStorageWindow)(target));
            return;
            case 2:
            this.ScaleWindow = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 3:
            
            #line 57 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 60 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 73 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            ((System.Windows.Media.Animation.Storyboard)(target)).Completed += new System.EventHandler(this.Timeline_OnCompleted);
            
            #line default
            #line hidden
            return;
            case 6:
            this.TxtPathSelection = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 92 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectDirectory);
            
            #line default
            #line hidden
            return;
            case 8:
            this.TxtPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 97 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            this.TxtPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.TxtPassword_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.TxtPasswordCollapsed = ((System.Windows.Controls.TextBox)(target));
            
            #line 98 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            this.TxtPasswordCollapsed.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtPasswordCollapsed_TextChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CbHide = ((System.Windows.Controls.CheckBox)(target));
            
            #line 99 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            this.CbHide.Checked += new System.Windows.RoutedEventHandler(this.cbHide_Checked);
            
            #line default
            #line hidden
            
            #line 99 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            this.CbHide.Unchecked += new System.Windows.RoutedEventHandler(this.cbHide_Checked);
            
            #line default
            #line hidden
            return;
            case 11:
            this.PbHard = ((System.Windows.Controls.ProgressBar)(target));
            
            #line 102 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            this.PbHard.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.PbHard_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 111 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateStorageButton);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 112 "..\..\..\..\Views\CreatePasswordStorageWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

