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

namespace UiaAtkBridgeTest
{
	// this enum only contains very basic widgets that are almost completely the same
	// in the Gtk# and in the MWF world (complex ones will need a different test concept type)
	public enum BasicWidgetType
	{
		//widgets in both toolkits:
		Label,
		NormalButton,
		Window,
		CheckBox,
		ComboBoxDropDownList,
		ComboBoxDropDownEntry,
		RadioButton,
		StatusBar,
		TextBoxEntry,       // textbox single-line (gtk: entry)
		TextBoxView,        // textbox multi-line (gtk: textview)
		MaskedTextBoxEntry, // maskedtextbox (gtk: entry with visibility:false)
		MainMenuBar,
		ParentMenu,
		VScrollBar,
		HScrollBar,
		ProgressBar,
		Spinner,
		
		//widgets only in MWF:
		ComboBoxSimple,
		ListBox,
		ListItem,
		CheckedListBox,
		CheckedListItem,
		TabControl,
		TabPage,
		ListView,
		GroupBox,
		PictureBox,
		ToolStripLabel
	}

//	public class MenuLayout
//	{
//		private string labelMenu;
//		private List<MenuLayout> subMenus;
//
//		public MenuLayout (string name, params MenuLayout[] submenus)
//		{
//			labelMenu = name;
//
//			subMenus = new List <MenuLayout> (submenus);
//		}
//		
//		public string LabelMenu { get { return labelMenu; } }
//		public List<MenuLayout> SubMenus { get { return subMenus; } }
//	}
	
	public static class Misc
	{
		internal static bool HasReadOnlyText (BasicWidgetType type) {
			if ((type == BasicWidgetType.TextBoxEntry) ||
			    (type == BasicWidgetType.TextBoxView))
				return false;
			return true;
		}

		public static string LookForParentDir (string pattern) {
			string imgDir = System.IO.Directory.GetCurrentDirectory ();
			
			while (imgDir != "/"){
				if (System.IO.Directory.GetFiles (imgDir, pattern).Length == 0)
					imgDir = System.IO.Path.GetFullPath (System.IO.Path.Combine (imgDir, ".."));
	
				else
					break;
				
				string samples = System.IO.Path.Combine (System.IO.Path.Combine (imgDir, "test"), "samples");
				if (System.IO.Directory.Exists (samples)) { 
					if (System.IO.Directory.GetFiles (samples, pattern).Length > 0) {
						imgDir = System.IO.Path.GetFullPath (samples);
						break;
					}
				}
			}
	
			if (imgDir != "/")
				return imgDir;
	
			return null;
		}
	}
}