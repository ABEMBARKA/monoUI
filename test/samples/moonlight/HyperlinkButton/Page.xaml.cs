﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HyperlinkButtonSample
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            hyperlink_blank.Content = "Click here\r\nto open OpenSUSE in New Window";
        }
    }
}
