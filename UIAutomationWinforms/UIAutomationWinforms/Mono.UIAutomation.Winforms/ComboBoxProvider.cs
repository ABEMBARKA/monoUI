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
using System.Windows;
using System.Windows.Automation;
using System.Collections.Generic;
using SWF = System.Windows.Forms;
using System.Windows.Automation.Provider;
using AEIds = System.Windows.Automation.AutomationElementIdentifiers;

using Mono.Unix;
using Mono.UIAutomation.Winforms.Behaviors;
using Mono.UIAutomation.Winforms.Behaviors.ListItem;
using Mono.UIAutomation.Winforms.Behaviors.ComboBox;
using Mono.UIAutomation.Winforms.Navigation;
using Mono.UIAutomation.Winforms.Events;
using Mono.UIAutomation.Winforms.Events.ComboBox;

namespace Mono.UIAutomation.Winforms
{

	internal class ComboBoxProvider : FragmentRootControlProvider
	{

		#region Constructor
		
		public ComboBoxProvider (SWF.ComboBox combobox) : base (combobox)
		{
			comboboxControl = combobox;
			comboboxControl.DropDownStyleChanged += OnDropDownStyleChanged;

			listboxProvider = new ComboBoxProvider.ComboBoxListBoxProvider (comboboxControl,
			                                                                this);
			listboxProvider.Initialize ();
		}
		
		#endregion

		#region Public Properties

		public ListProvider ListProvider {
			get { return listboxProvider; }
		}

		#endregion
		
		#region SimpleControlProvider: Specializations

		protected override object GetProviderPropertyValue (int propertyId)
		{
			if (propertyId == AEIds.ControlTypeProperty.Id)
				return ControlType.ComboBox.Id;
			else if (propertyId == AEIds.IsKeyboardFocusableProperty.Id)
				return true;
			else if (propertyId == AEIds.LocalizedControlTypeProperty.Id)
				return Catalog.GetString ("combo box");
			else if (propertyId == AEIds.HasKeyboardFocusProperty.Id
				 && comboboxControl.DropDownStyle == SWF.ComboBoxStyle.DropDown)
				return false;
			else
				return base.GetProviderPropertyValue (propertyId);
		}

		public override void Initialize ()
		{
			base.Initialize ();

			SetBehavior (SelectionPatternIdentifiers.Pattern,
			             new SelectionProviderBehavior (this));
		}
		
		public override void Terminate ()
		{
			base.Terminate ();
			
			comboboxControl.DropDownStyleChanged -= OnDropDownStyleChanged;
		}	

		#endregion		

		#region FragmentRootControlProvider: Specializations
		
		public override IRawElementProviderFragment GetFocus ()
		{
			return listboxProvider.GetItemProviderFrom (listboxProvider, 
			                                            comboboxControl.SelectedItem);
		}		
		
		public override IRawElementProviderFragment ElementProviderFromPoint (double x, double y)
		{
			throw new NotImplementedException ();
		}

		#endregion
		
		#region ListProvider: Specializations
		
		public override void InitializeChildControlStructure ()
		{
			OnNavigationChildAdded (false, listboxProvider);
			UpdateBehaviors (false);
		}
		
		public override void FinalizeChildControlStructure ()
		{
			if (buttonProvider != null) {
				OnNavigationChildRemoved (false, buttonProvider);
				buttonProvider.Terminate ();
				buttonProvider = null;
			}
			if (listboxProvider != null) {
				OnNavigationChildRemoved (false, listboxProvider);
				listboxProvider.Terminate ();
				listboxProvider = null;
			}
			if (textboxProvider != null) {
				OnNavigationChildRemoved (false, textboxProvider);
				textboxProvider.Terminate ();
				textboxProvider = null;
			}
		}

		#endregion

		#region Private Methods
		
		private void OnDropDownStyleChanged (object sender, EventArgs args)
		{
			UpdateBehaviors (true);
		}
		
		private void UpdateBehaviors (bool generateEvent) 
		{
			if (comboboxControl.DropDownStyle == SWF.ComboBoxStyle.Simple) {
				SetBehavior (ExpandCollapsePatternIdentifiers.Pattern, 
				             null);
				SetBehavior (ValuePatternIdentifiers.Pattern,
				             new ValueProviderBehavior (this));

				SetEvent (ProviderEventType.AutomationElementHasKeyboardFocusProperty,
					  new AutomationHasKeyboardFocusPropertyEvent (this));
				
				TerminateButtonProvider (generateEvent);
				InitializeEditProvider (generateEvent);
			} else if (comboboxControl.DropDownStyle == SWF.ComboBoxStyle.DropDown) {
				SetBehavior (ExpandCollapsePatternIdentifiers.Pattern,
				             new ExpandCollapseProviderBehavior (this));
				SetBehavior (ValuePatternIdentifiers.Pattern,
				             new ValueProviderBehavior (this));

				SetEvent (ProviderEventType.AutomationElementHasKeyboardFocusProperty,
					  null);
				
				InitializeButtonProvider (generateEvent);
				InitializeEditProvider (generateEvent);
			} else if (comboboxControl.DropDownStyle == SWF.ComboBoxStyle.DropDownList) {
				SetBehavior (ExpandCollapsePatternIdentifiers.Pattern,
				             new ExpandCollapseProviderBehavior (this));
				SetBehavior (ValuePatternIdentifiers.Pattern, 
				             null);

				SetEvent (ProviderEventType.AutomationElementHasKeyboardFocusProperty,
					  new AutomationHasKeyboardFocusPropertyEvent (this));
				
				InitializeButtonProvider (generateEvent);
				TerminateEditProvider (generateEvent);
			} 
		}
		
		private void InitializeEditProvider (bool generateEvent)
		{
			if (textboxProvider == null) {
				textboxProvider = new ComboBoxTextBoxProvider (comboboxControl.UIATextBox, this);
				textboxProvider.Initialize ();

				OnNavigationChildAdded (generateEvent, textboxProvider);
			}
		}

		private void TerminateEditProvider (bool generateEvent)
		{
			if (textboxProvider != null) {
				OnNavigationChildRemoved (generateEvent, textboxProvider);
				textboxProvider.Terminate ();
				textboxProvider = null;
			}
		}
		
		private void InitializeButtonProvider (bool generateEvent)
		{
			if (buttonProvider == null) {
				buttonProvider = new ComboBoxProvider.ComboBoxButtonProvider (comboboxControl,
				                                                              this);
				buttonProvider.Initialize ();
				OnNavigationChildAdded (generateEvent, buttonProvider);
			}
		}
		
		private void TerminateButtonProvider (bool generateEvent)
		{
			if (buttonProvider != null) {
				OnNavigationChildRemoved (generateEvent, buttonProvider);
				buttonProvider.Terminate ();
				buttonProvider = null;
			}
		}
		
		#endregion
			
		#region Private Fields
		
		private SWF.ComboBox comboboxControl;
		private ComboBoxProvider.ComboBoxButtonProvider buttonProvider;
		private ComboBoxProvider.ComboBoxListBoxProvider listboxProvider;
		private TextBoxProvider textboxProvider;
		
		#endregion
		
		#region Internal Class: ListBox provider
		
		internal class ComboBoxListBoxProvider 
			: ListProvider, IScrollBehaviorSubject
		{
			
			public ComboBoxListBoxProvider (SWF.ComboBox control, 
			                                ComboBoxProvider provider)
				: base (control)
			{
				comboboxControl = control;
				comboboxProvider = provider;

				// To keep track of the internal Control that represents the ListBox
				comboboxControl.DropDownStyleChanged += OnDropDownStyleChanged;
				comboboxControl.DropDown += OnDropDownAndDropClosed;
				comboboxControl.DropDownClosed += OnDropDownAndDropClosed;

				listboxControl = ListBoxControl;
			}
			
			public override IRawElementProviderFragmentRoot FragmentRoot {
				get { return comboboxProvider; }
			}
	
			protected override object GetProviderPropertyValue (int propertyId)
			{
				if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id)
					return ControlType.List.Id;
				else if (propertyId == AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id)
					return true;
				else if (propertyId == AutomationElementIdentifiers.HasKeyboardFocusProperty.Id)
					return true;
				else if (propertyId == AutomationElementIdentifiers.LocalizedControlTypeProperty.Id)
					return Catalog.GetString ("list");
				else if (propertyId == AutomationElementIdentifiers.IsScrollPatternAvailableProperty.Id)
					return IsBehaviorEnabled (ScrollPatternIdentifiers.Pattern);
				else if (propertyId == AutomationElementIdentifiers.IsTablePatternAvailableProperty.Id)
					return false;
				else if (propertyId == AutomationElementIdentifiers.BoundingRectangleProperty.Id) {
					//We try to get internal UIAComboListBox in SWF.ComboBox if returns null we 
					//use the SWF.ComboBox bounds
					SWF.Control listboxControl = ListBoxControl;
					if (listboxControl == null)
						return comboboxProvider.GetProviderPropertyValue (propertyId);
					else
						return Helper.GetControlScreenBounds (listboxControl.Bounds, listboxControl);
				} else if (propertyId == AutomationElementIdentifiers.IsOffscreenProperty.Id) {
					if (comboboxControl.DropDownStyle == SWF.ComboBoxStyle.Simple)
						return false;
					
					IExpandCollapseProvider pattern 
						= comboboxProvider.GetPatternProvider (ExpandCollapsePatternIdentifiers.Pattern.Id) as IExpandCollapseProvider;
					return pattern != null && pattern.ExpandCollapseState == ExpandCollapseState.Collapsed;
				} else
					return base.GetProviderPropertyValue (propertyId);
			}

			public override int SelectedItemsCount {
				get { return comboboxControl.SelectedIndex == -1 ? 0 : 1; }
			}
			
			public override int ItemsCount {
				get { return comboboxControl.Items.Count; }
			}

			public override int IndexOfObjectItem (object objectItem)
			{
				return comboboxControl.Items.IndexOf (objectItem);
			}	

			public override ListItemProvider[] GetSelectedItems ()
			{
				if (comboboxControl == null || comboboxControl.SelectedIndex == -1)
					return new ListItemProvider [0];
				else
					return new ListItemProvider [] { GetItemProviderFrom (this,
					                                                      comboboxControl.SelectedItem) };
			}

			public override void SelectItem (ListItemProvider item)
			{
				if (ContainsItem (item) == true)
					comboboxControl.SelectedIndex = item.Index;
			}
	
			public override void UnselectItem (ListItemProvider item)
			{
			}
			
			public override bool IsItemSelected (ListItemProvider item)
			{
				return ContainsItem (item) == false 
					? false : item.Index == comboboxControl.SelectedIndex;
			}

			public override void InitializeChildControlStructure ()
			{
				comboboxControl.Items.UIACollectionChanged += OnCollectionChanged;

				foreach (object objectItem in comboboxControl.Items) {
					ListItemProvider item = GetItemProviderFrom (this, objectItem);
					OnNavigationChildAdded (false, item);
				}

				InitializeObserver (true);
			}

			public override void FinalizeChildControlStructure ()
			{
				base.FinalizeChildControlStructure ();

				comboboxControl.Items.UIACollectionChanged -= OnCollectionChanged;
				if (observer != null) {
					observer.Terminate ();
					observer = null;
				}
			}

			public override void ScrollItemIntoView (ListItemProvider item) 
			{ 
				//FIXME: Implement
				throw new NotImplementedException ();
			}

			public override object GetItemPropertyValue (ListItemProvider item,
			                                             int propertyId)
			{
				if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
					return item.ObjectItem.ToString ();

				int topItem = -1;
				if (ListBoxControl != null)
					topItem = ListBoxControl.UIATopItem;

				if (ContainsItem (item) == false)
					return null;
				else if (propertyId == AutomationElementIdentifiers.HasKeyboardFocusProperty.Id)
					return comboboxControl.Focused && item.Index == comboboxControl.SelectedIndex;
				else if (propertyId == AutomationElementIdentifiers.BoundingRectangleProperty.Id) {
					//FIXME: We need to improve this
					int index = item.Index;
					System.Drawing.Rectangle rectangle = System.Drawing.Rectangle.Empty;
					System.Drawing.Rectangle bounds = System.Drawing.Rectangle.Empty;

					if (ListBoxControl == null)
						bounds = comboboxControl.Bounds;
					else
						bounds = ListBoxControl.Bounds;

					int itemHeight = comboboxControl.GetItemHeight (0);// TODO: always true?
					rectangle.Height = comboboxControl.GetItemHeight (index);
					rectangle.Width = bounds.Width;
					rectangle.X = bounds.X;
					rectangle.Y = bounds.Y + (index * itemHeight) - (topItem * itemHeight);// decreaseY;

					if (ListBoxControl == null)
						return Helper.GetControlScreenBounds (rectangle, comboboxControl);
					else
						return Helper.GetControlScreenBounds (rectangle, ListBoxControl);
				} else if (propertyId == AutomationElementIdentifiers.IsOffscreenProperty.Id) {
					if (comboboxControl.SelectedIndex == item.Index)
						return false;
					
					if (topItem == -1 || !ListBoxControl.Visible)
						return !(comboboxControl.SelectedIndex == item.Index);

					int lastItem = ListBoxControl.UIALastItem;				
					if ((item.Index >= topItem && item.Index < lastItem) 
					    || (item.Index == lastItem && comboboxControl.Items.Count == lastItem + 1))
						return false;
					else
						return true;
				} else
					return null;
			}
			
			internal override IProviderBehavior GetBehaviorRealization (AutomationPattern behavior)
			{
				if (behavior == SelectionPatternIdentifiers.Pattern)
					return new ListBoxSelectionProviderBehavior (this, 
					                                             comboboxProvider);
				else 
					return null;
			}		
			
			public override IProviderBehavior GetListItemBehaviorRealization (AutomationPattern behavior,
			                                                                  ListItemProvider listItem)
			{
				if (behavior == SelectionItemPatternIdentifiers.Pattern)
					return new ListItemSelectionItemProviderBehavior (listItem);
				else
					return base.GetListItemBehaviorRealization (behavior, listItem);
			}

			public override IConnectable GetListItemEventRealization (ProviderEventType eventType, 
			                                                          ListItemProvider prov)
			{
				if (eventType == ProviderEventType.AutomationElementIsOffscreenProperty)
				    return new ListItemAutomationIsOffscreenPropertyEvent (prov);
				else
					return base.GetListItemEventRealization (eventType, prov);
				//FIXME: Return AutomationIsKeyboardFocusablePropertyEvent
			}

			public SWF.ComboBox.ComboListBox ListBoxControl {
				get { return comboboxControl.UIAComboListBox; }
			}

			public IScrollBehaviorObserver ScrollBehaviorObserver {
				get { return observer; }
			}
			
			public FragmentControlProvider GetScrollbarProvider (SWF.ScrollBar scrollbar)
			{
				return new ComboBoxListBoxScrollBarProvider (scrollbar, this);
			}

			private void OnDropDownStyleChanged (object sender, EventArgs args)
			{
				InitializeObserver (false);
			}

			private void InitializeObserver (bool forceInitialization)
			{
				SWF.Control oldListBoxControl = listboxControl;
				if (ListBoxControl != oldListBoxControl || forceInitialization) {
					if (observer != null) {
						observer.ScrollPatternSupportChanged -= OnScrollPatternSupportChanged;
						observer.Terminate ();
					}

					// FIXME: Replace with an internal UIA-like property in SWF
					if (ListBoxControl != null) {
						observer = new ScrollBehaviorObserver (this, null,
						                                       ListBoxControl.UIAVScrollBar);
						observer.ScrollPatternSupportChanged += OnScrollPatternSupportChanged;
						observer.Initialize ();
						UpdateScrollBehavior ();
					}

					listboxControl = ListBoxControl;
				}
			}

			private void OnScrollPatternSupportChanged (object sender, EventArgs args)
			{
				UpdateScrollBehavior ();
			}

			private void OnDropDownAndDropClosed (object sender, EventArgs args)
			{
				InitializeObserver (false);
			}
			
			private void UpdateScrollBehavior ()
			{
				//FIXME: Implement ScrollPattern
//				if (observer.SupportsScrollPattern == true)
//					SetBehavior (ScrollPatternIdentifiers.Pattern,
//					             new ScrollProviderBehavior (this));
//				else
//					SetBehavior (ScrollPatternIdentifiers.Pattern, null);
			}

			private SWF.ComboBox comboboxControl;
			private ComboBoxProvider comboboxProvider;
			private SWF.Control listboxControl;
			private ScrollBehaviorObserver observer;
		}
		
		#endregion

		#region Internal Class: Button provider

		internal class ComboBoxButtonProvider : FragmentControlProvider
		{

			public ComboBoxButtonProvider (SWF.Control control,
			                               ComboBoxProvider provider)
				: base (control)
			{
				this.provider = provider;
			}

			public override void Initialize ()
			{
				base.Initialize ();

				SetBehavior (InvokePatternIdentifiers.Pattern,
				             new ButtonInvokeBehavior (provider));
			}
	
			protected override object GetProviderPropertyValue (int propertyId)
			{
				if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id)
					return ControlType.Button.Id;
				else if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
					return Catalog.GetString ("Drop Down Button");
				else if (propertyId == AutomationElementIdentifiers.LabeledByProperty.Id)
					return null;
				else
					return base.GetProviderPropertyValue (propertyId);
			}

			private ComboBoxProvider provider;
		}
		
		#endregion

		#region Internal Class: ScrollBar provider

		internal class ComboBoxListBoxScrollBarProvider : ScrollBarProvider
		{
			public ComboBoxListBoxScrollBarProvider (SWF.ScrollBar scrollbar,
			                                         ComboBoxListBoxProvider listboxProvider)
				: base (scrollbar)
			{
				this.listboxProvider = listboxProvider;
			}
			
			public override IRawElementProviderFragmentRoot FragmentRoot {
				get { return listboxProvider; }
			}			
			
			protected override object GetProviderPropertyValue (int propertyId)
			{
				if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
					return Catalog.GetString ("Vertical Scroll Bar");
				else
					return base.GetProviderPropertyValue (propertyId);
			}
			
			private ComboBoxListBoxProvider listboxProvider;
		}
		
		#endregion
		
		#region Internal Class: TextBox provider
	
		internal class ComboBoxTextBoxProvider : TextBoxProvider
		{
			public ComboBoxTextBoxProvider (SWF.TextBox textbox,
			                                ComboBoxProvider comboProvider)
				: base (textbox)
			{
				this.comboProvider = comboProvider;
			}

			public override void Initialize ()
			{
				base.Initialize ();

				SetEvent (ProviderEventType.AutomationElementHasKeyboardFocusProperty,
				          new TextBoxAutomationHasKeyboardFocusPropertyEvent (this, comboProvider));
			}

			protected override object GetProviderPropertyValue (int propertyId)
			{
				if (propertyId == AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id) {
					// This control cannot be selected, but
					// it can still be keyboard focused, so
					// we override it manually.
					return Control.CanFocus;
				}

				return base.GetProviderPropertyValue (propertyId);
			}

			private ComboBoxProvider comboProvider;
		}

		#endregion

	}
}
