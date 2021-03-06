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
//	Brad Taylor <brad@getcoded.net>
// 

using System;
using System.Windows.Automation;
using SWF = System.Windows.Forms;
using System.Windows.Automation.Provider;

using Mono.UIAutomation.Winforms.Events;

namespace Mono.UIAutomation.Winforms.Events.DateTimePicker
{
	internal class PartRangeValuePatternIsReadOnlyEvent
		: BaseAutomationPropertyEvent
	{
#region Constructor
		public PartRangeValuePatternIsReadOnlyEvent (FragmentControlProvider provider,
		                                             DateTimePickerProvider dateTimePickerProvider) 
			: base (provider, RangeValuePatternIdentifiers.IsReadOnlyProperty)
		{
			this.dateTimePicker
				= (SWF.DateTimePicker) dateTimePickerProvider.Control;
		}
#endregion
		
#region IConnectable Overrides
		public override void Connect ()
		{
			dateTimePicker.UIAChecked
				+= new EventHandler (OnUIAChecked);
		}

		public override void Disconnect ()
		{
			dateTimePicker.UIAChecked
				-= new EventHandler (OnUIAChecked);
		}
#endregion 
		
#region Private Methods
		private void OnUIAChecked (object o, EventArgs args)
		{
			RaiseAutomationPropertyChangedEvent ();
		}
#endregion

#region Private Fields
		private SWF.DateTimePicker dateTimePicker;
#endregion
	}
}
