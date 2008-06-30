Index: class/Managed.Windows.Forms/System.Windows.Forms/ScrollBar.cs
===================================================================
--- class/Managed.Windows.Forms/System.Windows.Forms/ScrollBar.cs	(revision 106905)
+++ class/Managed.Windows.Forms/System.Windows.Forms/ScrollBar.cs	(working copy)
@@ -80,6 +80,21 @@
 		bool thumb_entered;
 		#endregion	// Local Variables
 
+		#region UIA Framework Events	
+#if NET_2_0
+    		//UIA Framework events for Invoke Control Pattern used in
+    		//the ScrollBar Provider children buttons.
+    		// - LargeIncrementCalled. Used by the LargeIncrement UIA Button (Space between Thumb and bottom/right Button)
+    		// - LargeIncrementCalled. Used by the LargeDecrement UIA Button (Space between Thumb and top/left Button)
+    		// - SmallIncrementCalled. Used by the SmallIncrement UIA Button (bottom/right Button)
+    		// - SmallDecrementCalled. Used by the SmallDecrement UIA Button (top/left Button)
+    		internal event EventHandler LargeIncrementCalled;
+    		internal event EventHandler LargeDecrementCalled;
+    		internal event EventHandler SmallIncrementCalled;
+    		internal event EventHandler SmallDecrementCalled;
+#endif
+		#endregion
+
 		private enum TimerType
 		{
 			HoldButton,
@@ -788,6 +803,14 @@
 			event_args = new ScrollEventArgs (ScrollEventType.EndScroll, Value);
 			OnScroll (event_args);
     			Value = event_args.NewValue;
+		
+#if NET_2_0
+			//UIA Framework event invoked when the "LargeIncrement 
+			//Button" is "clicked" either by using the Invoke Pattern
+			//or the space between the thumb and the bottom/right button
+    			if (LargeIncrementCalled != null)
+	    			LargeIncrementCalled (this, null);
+#endif
     		}
 
     		private void LargeDecrement ()
@@ -802,6 +825,14 @@
 			event_args = new ScrollEventArgs (ScrollEventType.EndScroll, Value);
 			OnScroll (event_args);
     			Value = event_args.NewValue;
+    			
+#if NET_2_0
+			//UIA Framework event invoked when the "LargeDecrement 
+			//Button" is "clicked" either by using the Invoke Pattern
+			//or the space between the thumb and the top/left button
+    			if (LargeDecrementCalled != null)
+	    			LargeDecrementCalled (this, null);
+#endif
     		}
 
     		private void OnResizeSB (Object o, EventArgs e)
@@ -1296,6 +1327,14 @@
 			event_args = new ScrollEventArgs (ScrollEventType.EndScroll, Value);
 			OnScroll (event_args);
 			Value = event_args.NewValue;
+			
+#if NET_2_0
+			//UIA Framework event invoked when the "SmallIncrement 
+			//Button" (a.k.a bottom/right button) is "clicked" either
+			// by using the Invoke Pattern or the button itself
+    			if (SmallIncrementCalled != null)
+	    			SmallIncrementCalled (this, null);
+#endif
     		}
 
     		private void SmallDecrement ()
@@ -1310,6 +1349,14 @@
 			event_args = new ScrollEventArgs (ScrollEventType.EndScroll, Value);
 			OnScroll (event_args);
 			Value = event_args.NewValue;
+			
+#if NET_2_0
+			//UIA Framework event invoked when the "SmallDecrement 
+			//Button" (a.k.a top/left button) is "clicked" either
+			// by using the Invoke Pattern or the button itself
+    			if (SmallDecrementCalled != null)
+	    			SmallDecrementCalled (this, null);
+#endif			
     		}
 
     		private void SetHoldButtonClickTimer ()
