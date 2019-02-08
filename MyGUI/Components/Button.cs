using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUI.Session;
using MyGUI.Utilities;
using static MyGUI.Session.Settings.Appearance.ComponentStyle;
using static MyGUI.Session.Settings.Appearance.ComponentStyle.SeparatorStyle;
using static MyGUI.Session.Settings.Console;
using static MyGUI.Session.Console;
using static MyGUI.Session.Resources;

namespace MyGUI
{
	public class Button : PrimitiveComponent
	{
		public Button(int height, int width, string name, string caption, Action onClick = null)
		{

		}

		public const int MinHeight = 1;
		public const int MinWidth = 6;
		public const int DefaultHeight = MinHeight;

		public event Action OnClick;
		public Label LabelComponent { get; set; }

		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}

		public override void UpdateRenderBuffer()
		{
			throw new NotImplementedException();
		}
	}
}
