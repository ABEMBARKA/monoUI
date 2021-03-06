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
//      Andres G. Aragoneses <aaragoneses@novell.com>
// 

using System;
using System.Collections.Generic;

using System.Windows.Automation;
using Mono.UIAutomation.Services;
using System.Windows.Automation.Provider;

namespace UiaAtkBridge
{
	public class ComboBoxDropDown : ComboBox, Atk.IActionImplementor
	{
		private const int MENU_ELEMENT_POS_INSIDE_COMBOBOX = 0;
		private string actionDescription = null;
		private string actionName = "press";
		
		private IRawElementProviderFragmentRoot 	provider;
		private IExpandCollapseProvider			expandCollapseProvider;
		
		private ComboBoxOptions InnerMenu {
			get { return (ComboBoxOptions)RefAccessibleChild (MENU_ELEMENT_POS_INSIDE_COMBOBOX); }
		}
		
		public ComboBoxDropDown (IRawElementProviderSimple provider) : base (provider)
		{
			this.provider = provider as IRawElementProviderFragmentRoot;
			if (this.provider == null)
				throw new ArgumentException ("Provider should be IRawElementProviderFragmentRoot");
			
			expandCollapseProvider = (IExpandCollapseProvider)provider.GetPatternProvider (ExpandCollapsePatternIdentifiers.Pattern.Id);
		}

		public bool IsEditable {
			//if we have 1 child, we're a ComboBoxDropDown, if we have 2, we're a ComboBoxDropDownEntry
			get { return NAccessibleChildren == 2; }
		}

		protected override Atk.StateSet OnRefStateSet ()
		{
			Atk.StateSet states = base.OnRefStateSet ();

			if (states.ContainsState (Atk.StateType.Defunct))
				return states;

			states.RemoveState (Atk.StateType.ManagesDescendants); //our base provides this
			
			//FIXME: figure out why Gail comboboxes don't like this state
			states.RemoveState (Atk.StateType.Focusable);
			return states;
		}
		
		public int NActions {
			get { return 1; }
		}
		
		public bool DoAction (int i)
		{
			if (i != 0)
				return false;
			try {
				switch (expandCollapseProvider.ExpandCollapseState) {
				case ExpandCollapseState.Collapsed:
					try {
						expandCollapseProvider.Expand ();
					} catch (ElementNotEnabledException e) {
						Log.Debug (e);
						return false;
					}
					break;
				case ExpandCollapseState.Expanded:
					try {
						expandCollapseProvider.Collapse ();
					} catch (ElementNotEnabledException e) {
						Log.Debug (e);
						return false;
					}
					break;
				default:
					throw new NotSupportedException ("A combobox should not have an ExpandCollapseState different than Collapsed/Expanded");
				}
				return true;
			} catch (ElementNotEnabledException) { }
			return false;
		}

		internal ExpandCollapseState ExpandCollapseState {
			get { return expandCollapseProvider.ExpandCollapseState; }
		}

		public string GetDescription (int i)
		{
			if (i != 0)
				return null;
			return actionDescription;
		}

		public string GetName (int i)
		{
			if (i != 0)
				return null;
			return actionName;
		}

		public string GetKeybinding (int i)
		{
			//TODO:
			return null;
		}

		public bool SetDescription (int i, string desc)
		{
			if (i != 0)
				return false;
			actionDescription = desc;
			return true;
		}

		public string GetLocalizedName (int i)
		{
			if (i != 0)
				return null;
			return actionName;
		}
		
		public override void RaiseAutomationEvent (AutomationEvent eventId, AutomationEventArgs e)
		{
			base.RaiseAutomationEvent (eventId, e);
		}

		private Window fakeWindow = null;
		
		public override void RaiseAutomationPropertyChangedEvent (System.Windows.Automation.AutomationPropertyChangedEventArgs e)
		{
			if (e.Property.Id != ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty.Id) {
				base.RaiseAutomationPropertyChangedEvent (e);
				return;
			}

			ExpandCollapseState newState = (ExpandCollapseState)e.NewValue;
			if (newState == ExpandCollapseState.Expanded) {
				if (fakeWindow == null) {
					fakeWindow = new DropDownWindow ();
					fakeWindow.AddOneChild ((Adapter)RefAccessibleChild (0));
				}
				TopLevelRootItem.Instance.AddOneChild (fakeWindow);
				TopLevelRootItem.Instance.CheckAndHandleNewActiveWindow (fakeWindow);
			} else if (newState == ExpandCollapseState.Collapsed) {
				Atk.Object realWindow = this;
				while (realWindow != null && !(realWindow is Window))
					realWindow = realWindow.Parent;
				if (realWindow != null)
					TopLevelRootItem.Instance.CheckAndHandleNewActiveWindow (fakeWindow, (Window)realWindow);
				TopLevelRootItem.Instance.RemoveChild (fakeWindow, false);
			}
			InnerMenu.RaiseExpandedCollapsed ();
		}


		public override void RaiseStructureChangedEvent (object provider, StructureChangedEventArgs e)
		{
			// TODO
			base.RaiseStructureChangedEvent (provider, e);
		}

		internal override Window PrivateWindow {
			get {
				return (expandCollapseProvider != null && expandCollapseProvider.ExpandCollapseState == ExpandCollapseState.Expanded? fakeWindow: null);
			}
		}
	}

	public class DropDownWindow : Window
	{
		protected override Atk.StateSet OnRefStateSet ()
		{
			Atk.StateSet states = base.OnRefStateSet ();
			if (active) {
				states.AddState (Atk.StateType.Enabled);
				states.AddState (Atk.StateType.Sensitive);
				states.AddState (Atk.StateType.Showing);
				states.AddState (Atk.StateType.Visible);
			}
			return states;
		}
	}
}
