// DataGrid.cs: DataGrid control class wrapper.
//
// This program is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License version 2 as published by the
// Free Software Foundation.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with
// this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
//
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
//
// Authors:
//	Ray Wang  (rawang@novell.com)
//	Felicia Mu  (fxmu@novell.com)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace Mono.UIAutomation.TestFramework
{
	public class DataGrid : Element
	{
		public static readonly ControlType UIAType = ControlType.DataGrid;

		public DataGrid (AutomationElement elm)
			: base (elm)
		{
		}

		// The method and properties of GridPattern
		public AutomationElement GetItem (int row, int column)
		{
			return GetItem (row, column, true);
		}

		public AutomationElement GetItem (int row, int column, bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Get element from row {0}, col {1} of {2}.",
				                                       row, column, this.NameAndType));

			GridPattern gp = (GridPattern) element.GetCurrentPattern (GridPattern.Pattern);
			return gp.GetItem (row, column);
		}

		public int RowCount {
			get { return (int) element.GetCurrentPropertyValue (GridPattern.RowCountProperty); }
		}

		public int ColumnCount {
			get { return (int) element.GetCurrentPropertyValue (GridPattern.ColumnCountProperty); }
		}

		// The methods and properties of MultipleViewPattern
		public string GetViewName (int viewId)
		{
			return GetViewName(viewId, true);
		}

		public string GetViewName (int viewId, bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Get the view name which viewID is {0}.", viewId));

			MultipleViewPattern mvp = (MultipleViewPattern) element.GetCurrentPattern (MultipleViewPattern.Pattern);
			return mvp.GetViewName (viewId);
		}

		public void SetCurrentView (int viewId)
		{
			SetCurrentView (viewId, true);
		}

		public void SetCurrentView (int viewId, bool log)
		{
			if (!this.SupportedViews.Contains(viewId))
			    throw new ArgumentOutOfRangeException();

			if (log)
				procedureLogger.Action (string.Format ("Set current view to {0}.", GetViewName(viewId)));

			MultipleViewPattern mvp = (MultipleViewPattern) element.GetCurrentPattern (MultipleViewPattern.Pattern);
			mvp.SetCurrentView (viewId);
		}

		public int CurrentView {
			get { return (int) element.GetCurrentPropertyValue (MultipleViewPattern.CurrentViewProperty); }
		}

		public int [] SupportedViews {
			get { return (int []) element.GetCurrentPropertyValue (MultipleViewPattern.SupportedViewsProperty); }
		}

		// The methods and properties of ScrollPattern
		public void Scroll (ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			Scroll (horizontalAmount, verticalAmount, true);
		}

		public void Scroll (ScrollAmount horizontalAmount, ScrollAmount verticalAmount, bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Scroll {0} horizontally and {1} vertically.",
				                                       horizontalAmount.ToString (),
				                                       verticalAmount.ToString ()));

			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.Scroll (horizontalAmount, verticalAmount);
		}

		public void ScrollHorizontal (ScrollAmount amount)
		{
			ScrollHorizontal (amount, true);
		}

		public void ScrollHorizontal (ScrollAmount amount, bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Scroll {0} horizontally.", amount.ToString ()));

			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.ScrollHorizontal (amount);
		}

		public void ScrollVertical (ScrollAmount amount)
		{
			ScrollVertical (amount, true);
		}

		public void ScrollVertical (ScrollAmount amount, bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Scroll {0} vertically..", amount.ToString ()));

			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.ScrollVertical (amount);
		}

		public void SetScrollPercent (double horizontalPercent, double verticalPercent)
		{
			SetScrollPercent (horizontalPercent, verticalPercent, true);
		}

		public void SetScrollPercent (double horizontalPercent, double verticalPercent, bool log)
		{
			if (log)
				procedureLogger.Action (string.Format ("Set {0} {1}% horizontally and {2}% vertically.",
				                                       this.NameAndType, horizontalPercent, verticalPercent));

			ScrollPattern sp = (ScrollPattern) element.GetCurrentPattern (ScrollPattern.Pattern);
			sp.SetScrollPercent (horizontalPercent, verticalPercent);
		}

		public bool HorizontallyScrollable {
			get { return (bool) element.GetCurrentPropertyValue (ScrollPattern.HorizontallyScrollableProperty); }
		}

		public double HorizontalScrollPercent {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.HorizontalScrollPercentProperty); }
		}

		public double HorizontalViewSize {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.HorizontalViewSizeProperty); }
		}

		public bool VerticallyScrollable {
			get { return (bool) element.GetCurrentPropertyValue (ScrollPattern.VerticallyScrollableProperty); }
		}

		public double VerticalScrollPercent {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.VerticalScrollPercentProperty); }
		}

		public double VerticalViewSize {
			get { return (double) element.GetCurrentPropertyValue (ScrollPattern.VerticalViewSizeProperty); }
		}
	}
}