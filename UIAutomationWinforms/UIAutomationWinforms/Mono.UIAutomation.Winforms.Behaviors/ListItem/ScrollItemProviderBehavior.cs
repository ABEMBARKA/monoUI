// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//	Mario Carrion <mcarrion@novell.com>
// 

using System;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using System.Windows;
using Mono.UIAutomation.Winforms;
using Mono.UIAutomation.Winforms.Events;

namespace Mono.UIAutomation.Winforms.Behaviors.ListItem
{

	internal class ScrollItemProviderBehavior 
		: ProviderBehavior, IScrollItemProvider
	{
			
		#region Constructors

		public ScrollItemProviderBehavior (ListItemProvider provider)
			: base (provider)
		{
		}

		#endregion
		
		#region IProviderBehavior Interface

		public override AutomationPattern ProviderPattern { 
			get { return ScrollItemPatternIdentifiers.Pattern; }
		}
		
		public override void Connect ()
		{
			//Doesn't generate any UIA event
		}
		
		public override void Disconnect ()
		{
			//Doesn't generate any UIA event
		}
		
		#endregion

		#region IScrollItemProvider Interface
		
		public void ScrollIntoView ()
		{
			PerformScrollIntoView (((ListItemProvider) Provider).ListProvider);
		}

		#endregion
		
		#region Private Methods
		
		private void PerformScrollIntoView (IListProvider provider)
		{
			if (provider.Control.InvokeRequired == true) {
				provider.Control.BeginInvoke (new ScrollIntoViewDelegate (PerformScrollIntoView),
				                              new object [] { provider });
				return;
			}

			provider.ScrollItemIntoView ((ListItemProvider) Provider);
		}
		
		#endregion
	}
	
	delegate void ScrollIntoViewDelegate (ListProvider provider);
}
