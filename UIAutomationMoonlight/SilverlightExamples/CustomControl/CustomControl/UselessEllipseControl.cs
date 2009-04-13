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
using System.Windows.Automation.Peers;

namespace CustomControl
{
	public class UselessEllipseControl : UselessChildControl
	{
		public UselessEllipseControl ()
		{
		}

		protected override UIElement Child {
			get {
				if (ellipse == null)
					ellipse = new Ellipse ();
				return ellipse;
			}
		}

		Ellipse ellipse;
	}
}