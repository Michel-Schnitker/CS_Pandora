﻿#pragma checksum "..\..\Optionen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "92DA7B8C36AF9C8F0A84DB4C309B9AC99BA88B09C776307E31779A516B868C9A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Csharp_WPF_PandoraProjekt;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Csharp_WPF_PandoraProjekt {
    
    
    /// <summary>
    /// Window1
    /// </summary>
    public partial class Window1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label anzeigeProzentWasser;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label anzeigeProzentPlankton;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label anzeigeProzentFische;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label anzeigeProzentHaie;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider HeufigkeitWasser;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider HeufigkeitPlankton;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider HeufigkeitFische;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider HeufigkeitHaie;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button saveOptionen;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\Optionen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseOptionen;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Csharp_WPF_PandoraProjekt;component/optionen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Optionen.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.anzeigeProzentWasser = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.anzeigeProzentPlankton = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.anzeigeProzentFische = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.anzeigeProzentHaie = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.HeufigkeitWasser = ((System.Windows.Controls.Slider)(target));
            
            #line 21 "..\..\Optionen.xaml"
            this.HeufigkeitWasser.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.HeufigkeitWasser_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.HeufigkeitPlankton = ((System.Windows.Controls.Slider)(target));
            
            #line 22 "..\..\Optionen.xaml"
            this.HeufigkeitPlankton.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.HeufigkeitPlankton_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.HeufigkeitFische = ((System.Windows.Controls.Slider)(target));
            
            #line 23 "..\..\Optionen.xaml"
            this.HeufigkeitFische.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.HeufigkeitFische_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.HeufigkeitHaie = ((System.Windows.Controls.Slider)(target));
            
            #line 24 "..\..\Optionen.xaml"
            this.HeufigkeitHaie.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.HeufigkeitHaie_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.saveOptionen = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\Optionen.xaml"
            this.saveOptionen.Click += new System.Windows.RoutedEventHandler(this.saveOptionen_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CloseOptionen = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\Optionen.xaml"
            this.CloseOptionen.Click += new System.Windows.RoutedEventHandler(this.CloseOptionen_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
