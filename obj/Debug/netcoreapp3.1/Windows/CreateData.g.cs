﻿#pragma checksum "..\..\..\..\Windows\CreateData.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4B8ECDAA640A70F2094092314F04871A51ACD754"
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


namespace OlibPasswordManager.Windows {
    
    
    /// <summary>
    /// CreateData
    /// </summary>
    public partial class CreateData : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Windows\CreateData.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtPathSelection;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Windows\CreateData.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TxtPassword;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Windows\CreateData.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtPasswordCollapsed;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Windows\CreateData.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CbHide;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Windows\CreateData.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar PbHard;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/OlibPasswordManager;V1.2.0.160;component/windows/createdata.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\CreateData.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TxtPathSelection = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            
            #line 23 "..\..\..\..\Windows\CreateData.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PathSelection);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TxtPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 29 "..\..\..\..\Windows\CreateData.xaml"
            this.TxtPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.TxtPassword_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TxtPasswordCollapsed = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.CbHide = ((System.Windows.Controls.CheckBox)(target));
            
            #line 31 "..\..\..\..\Windows\CreateData.xaml"
            this.CbHide.Checked += new System.Windows.RoutedEventHandler(this.CollapsedPassword);
            
            #line default
            #line hidden
            
            #line 31 "..\..\..\..\Windows\CreateData.xaml"
            this.CbHide.Unchecked += new System.Windows.RoutedEventHandler(this.CollapsedPassword);
            
            #line default
            #line hidden
            return;
            case 6:
            this.PbHard = ((System.Windows.Controls.ProgressBar)(target));
            
            #line 34 "..\..\..\..\Windows\CreateData.xaml"
            this.PbHard.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.PbHard_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 42 "..\..\..\..\Windows\CreateData.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

