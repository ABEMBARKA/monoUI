#pragma checksum "C:\Users\Neville\Documents\Visual Studio 2008\Projects\DatePickerSample\DatePickerSample\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A9DEFBB9B2081B645AC3FB8242289AAB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace DatePickerSample {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock label1;
        
        internal System.Windows.Controls.DatePicker datepicker1;
        
        internal System.Windows.Controls.TextBlock label2;
        
        internal System.Windows.Controls.DatePicker datepicker2;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/DatePickerSample;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.label1 = ((System.Windows.Controls.TextBlock)(this.FindName("label1")));
            this.datepicker1 = ((System.Windows.Controls.DatePicker)(this.FindName("datepicker1")));
            this.label2 = ((System.Windows.Controls.TextBlock)(this.FindName("label2")));
            this.datepicker2 = ((System.Windows.Controls.DatePicker)(this.FindName("datepicker2")));
        }
    }
}