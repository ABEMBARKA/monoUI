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
//      Mike Gorse <mgorse@novell.com>
// 

using System;
using System.Windows.Automation;
using System.Collections.Generic;

using System.Windows.Automation.Provider;

namespace UiaAtkBridge
{
	internal class TableImplementorHelper
	{
		public TableImplementorHelper (ComponentParentAdapter resource)
		{
			this.resource = resource;
			
			tableProvider = (ITableProvider) resource.Provider.GetPatternProvider (TablePatternIdentifiers.Pattern.Id);;
		}

		private Adapter							resource;
		private ITableProvider					tableProvider = null;
		private TableGroupAggregator aggregator = null;

		internal IGridProvider GridProvider {
			get {
				IGridProvider grid = (IGridProvider) resource.Provider.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
				if (grid != null)
					return grid;
				if (aggregator == null) {
					IRawElementProviderFragment fragment = resource.Provider as IRawElementProviderFragment;
					aggregator = new TableGroupAggregator (fragment);
				}
				return aggregator;
			}
		}

		public Atk.Object RefAt (int row, int column)
		{
			IRawElementProviderSimple item = GridProvider.GetItem (row, column);
			if (item == null)
				return null;
Adapter i = AutomationBridge.GetAdapterForProviderLazy (item);
			return AutomationBridge.GetAdapterForProviderLazy (item);
		}

		public int GetIndexAt (int row, int column)
		{
			throw new NotImplementedException();
		}

		public int GetColumnAtIndex (int index)
		{
			Adapter child = resource.RefAccessibleChild (index) as Adapter;
			if (child != null && child.Provider != null) {
				IGridItemProvider g = (IGridItemProvider) child.Provider.GetPatternProvider (GridItemPatternIdentifiers.Pattern.Id);
				if (g == null)	// ie, if a group header
					return 0;
				return g.Column;
			}
			return -1;
		}

		public int GetRowAtIndex (int index)
		{
			int ret = 0;
			Adapter child = resource.RefAccessibleChild (index) as Adapter;
			if (child != null && child.Provider != null) {
				IGridItemProvider g = (IGridItemProvider) child.Provider.GetPatternProvider (GridItemPatternIdentifiers.Pattern.Id);
				if (g != null)
					ret = g.Row;
				return ret + RowAdjustment (child.Provider);;
			}
			return -1;
		}

		public int NColumns
		{
			get {
				if (GridProvider != null)
					return GridProvider.ColumnCount;
				return -1;
			}
		}

		public int NRows
		{
			get {
				if (GridProvider != null)
					return GridProvider.RowCount;
				return -1;
			}
		}

		public int GetColumnExtentAt (int row, int column)
		{
			IRawElementProviderSimple item = GridProvider.GetItem (row, column);
			int controlTypeId = (int) item.GetPropertyValue (AutomationElementIdentifiers.ControlTypeProperty.Id);
			if (controlTypeId == ControlType.Group.Id)
				return NColumns;
			IGridItemProvider g = (IGridItemProvider) item.GetPatternProvider (GridItemPatternIdentifiers.Pattern.Id);
			if (g != null)
				return g.ColumnSpan;
			return -1;
		}

		public int GetRowExtentAt (int row, int column)
		{
			IRawElementProviderSimple item = GridProvider.GetItem (row, column);
			IGridItemProvider g = (IGridItemProvider) item.GetPatternProvider (GridItemPatternIdentifiers.Pattern.Id);
			if (g != null)
				return g.RowSpan;
			return 1;
		}

		public Atk.Object Caption
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public string GetColumnDescription (int column)
		{
			throw new NotImplementedException();
		}

		public Atk.Object GetColumnHeader (int column)
		{
			// TODO: UIA supports multiple headers, but ATK doesn't
			IRawElementProviderSimple item = tableProvider.GetColumnHeaders()[0];
			return AutomationBridge.GetAdapterForProviderLazy (item);
		}

		public string GetRowDescription (int row)
		{
			throw new NotImplementedException();
		}

		public Atk.Object GetRowHeader (int row)
		{
			// TODO: UIA supports multiple headers, but ATK doesn't
			IRawElementProviderSimple item = tableProvider.GetRowHeaders()[0];
			return AutomationBridge.GetAdapterForProviderLazy (item);
		}

		public Atk.Object Summary
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public void SetColumnDescription (int column, string description)
		{
			throw new NotImplementedException();
		}

		public void SetColumnHeader (int column, Atk.Object header)
		{
			throw new NotImplementedException();
		}

		public void SetRowDescription (int row, string description)
		{
			throw new NotImplementedException();
		}

		public void SetRowHeader (int row, Atk.Object header)
		{
			throw new NotImplementedException();
		}

		public int GetSelectedColumns (out int selected)
		{
			throw new NotImplementedException();
		}

		public int GetSelectedRows (out int selected)
		{
			throw new NotImplementedException();
		}

		public bool IsColumnSelected (int column)
		{
			throw new NotImplementedException();
		}

		public bool IsRowSelected (int row)
		{
			throw new NotImplementedException();
		}

		public bool IsSelected (int row, int column)
		{
			throw new NotImplementedException();
		}

		public bool AddRowSelection (int row)
		{
			throw new NotImplementedException();
		}

		public bool RemoveRowSelection (int row)
		{
			throw new NotImplementedException();
		}

		public bool AddColumnSelection (int column)
		{
			throw new NotImplementedException();
		}

		public bool RemoveColumnSelection (int column)
		{
			throw new NotImplementedException();
		}

		internal int RowAdjustment (IRawElementProviderSimple provider)
		{
			if (GridProvider is TableGroupAggregator)
				return ((TableGroupAggregator)GridProvider).RowAdjustment ((IRawElementProviderFragment)provider);
			return 0;
		}
	}

	internal class TableGroupAggregator : IGridProvider
	{
		private IRawElementProviderFragment provider;

		public TableGroupAggregator (IRawElementProviderFragment provider)
		{
			this.provider = provider;
		}

		public IRawElementProviderFragment Provider {
			get { return provider; }
		}

		public int ColumnCount {
			get {
				int adj;
				IRawElementProviderSimple g = GetGridForRow (0, out adj);
				IGridProvider grid = null;
				if (g != null)
					grid = (IGridProvider) g.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
				if (g != null)
					return grid.ColumnCount;
				return -1;
			}
		}

		public int RowCount {
			get {
				int rows = 0;
				for (IRawElementProviderFragment child = Provider.Navigate (NavigateDirection.FirstChild); child != null; child = child.Navigate (NavigateDirection.NextSibling)) {
					IGridProvider grid = (IGridProvider) child.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
					if (grid != null)
						rows += grid.RowCount + 1;
				}
				return rows;
			}
		}

		public IRawElementProviderSimple GetItem (int row, int column)
		{
			int adj;
			IRawElementProviderSimple g = GetGridForRow (row, out adj);
			if (g == null)
				return null;
			if (adj == -1)
				return g;
			IGridProvider grid = (IGridProvider) g.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
			if (grid == null)
				return null;
			int localRow = row - adj;
			if (localRow < 0 || localRow >= grid.RowCount || column < 0 || column >= grid.ColumnCount)
				return null;
			return grid.GetItem (localRow, column);
		}

		internal IRawElementProviderSimple GetGridForRow (int row, out int adj)
		{
			IRawElementProviderFragment child;
			adj = 0;
			for (child = Provider.Navigate (NavigateDirection.FirstChild); child != null; child = child.Navigate (NavigateDirection.NextSibling)) {
				IGridProvider grid = (IGridProvider) child.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
				if (grid == null)
				continue;
				if (adj + grid.RowCount >= row) {
					if (adj == row)
						adj = -1;	// header
					adj++;	// skip group header
					return child;
				}
				adj += grid.RowCount + 1;
			}
			return null;
		}

		internal int RowAdjustment (IRawElementProviderFragment provider)
		{
			IRawElementProviderFragment cur = provider;
			int controlTypeId = (int) provider.GetPropertyValue (AutomationElementIdentifiers.ControlTypeProperty.Id);
			int adj = 0;
			IRawElementProviderFragment parent = provider.Navigate (NavigateDirection.Parent);
			int parentControlTypeId = (int) parent.GetPropertyValue (AutomationElementIdentifiers.ControlTypeProperty.Id);
			if (controlTypeId == ControlType.DataItem.Id) {
				if (parentControlTypeId == ControlType.DataGrid.Id)
					return 0;
				cur = parent;
				adj = 1;
			}
			for (cur = cur.Navigate (NavigateDirection.PreviousSibling); cur != null; cur = cur.Navigate (NavigateDirection.PreviousSibling)) {
				IGridProvider grid = (IGridProvider) cur.GetPatternProvider (GridPatternIdentifiers.Pattern.Id);
				if (grid != null)
					adj += grid.RowCount + 1;
			}
			return adj;
		}
	}
}