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
// Copyright (c) 2010 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//      Matt Guo <matt@mattguo.com>
// 

using System;
using System.Windows.Automation.Provider;

namespace System.Windows.Automation
{
	public struct ClientSideProviderDescription
	{
		public ClientSideProviderDescription (
			ClientSideProviderFactoryCallback clientSideProviderFactoryCallback,
			string className)
		: this (clientSideProviderFactoryCallback, className,
			null, ClientSideProviderMatchIndicator.None)
		{
		}

		public ClientSideProviderDescription (
			ClientSideProviderFactoryCallback clientSideProviderFactoryCallback,
			string className,
			string imageName,
			ClientSideProviderMatchIndicator flags)
		{
			this.ClientSideProviderFactoryCallback = clientSideProviderFactoryCallback;
			this.ClassName = className;
			this.ImageName = imageName;
			this.Flags = flags;
		}

		public string ClassName { get; private set; }
		public ClientSideProviderMatchIndicator Flags { get; private set; }
		public string ImageName { get; private set; }
		public ClientSideProviderFactoryCallback ClientSideProviderFactoryCallback { get; private set; }
	}

	[Flags]
	public enum ClientSideProviderMatchIndicator
	{
		None,
		AllowSubstringMatch,
		DisallowBaseClassNameMatch
	}

	public delegate IRawElementProviderSimple ClientSideProviderFactoryCallback (IntPtr hwnd, int idChild, int idObject);
}