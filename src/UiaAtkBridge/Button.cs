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
//      Sandy Armstrong <sanfordarmstrong@gmail.com>
//      Andrés G. Aragoneses <aaragoneses@novell.com>
// 

using System;
using System.Windows.Automation;
using System.Windows.Automation.Provider;

namespace UiaAtkBridge
{
	public class Button : Adapter
	{
		private object provider;
		private IRawElementProviderSimple simpleProvider;
		
		public Button (object provider)
		{
			this.provider = provider;
			Role = Atk.Role.PushButton;
			
			simpleProvider =
				(IRawElementProviderSimple) provider;
			string buttonText = (string) simpleProvider.GetPropertyValue (AutomationElementIdentifiers.NameProperty.Id);
			Name = buttonText;
			
			bool canFocus = (bool) simpleProvider.GetPropertyValue (AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id);
			if (canFocus)
				RefStateSet ().AddState (Atk.StateType.Selectable);
			else
				RefStateSet ().RemoveState (Atk.StateType.Selectable);
		}
		
		public override IRawElementProviderSimple Provider {
			get { return (IRawElementProviderSimple)provider; }
		}
		
		public override void RaiseAutomationEvent (AutomationEvent eventId, AutomationEventArgs e)
		{
			if (eventId == InvokePatternIdentifiers.InvokedEvent) {
				OnPressed ();
			}
		}

		public override void RaiseAutomationPropertyChangedEvent (AutomationPropertyChangedEventArgs e)
		{
			// TODO
		}
		
		private void OnPressed ()
		{
			NotifyStateChange ((ulong) Atk.StateType.Armed, true);
		}
		
		private void OnReleased ()
		{
			NotifyStateChange ((ulong) Atk.StateType.Armed, false);
		}
		
		private void OnDisabled ()
		{
			NotifyStateChange ((ulong) Atk.StateType.Sensitive, false);
		}
		
		private void OnEnabled ()
		{
			NotifyStateChange ((ulong) Atk.StateType.Sensitive, true);
		}
	}
}
