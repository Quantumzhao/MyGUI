using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Session;
using MyGUI.Utilities;
using static MyGUI.Session.Settings.Appearance.ComponentStyle.BorderStyle;
using static MyGUI.Session.Settings.Appearance.ComponentStyle.SeparatorStyle;
using static MyGUI.Session.Settings.Console;
using static MyGUI.Session.Console;
using static MyGUI.Session.Resources;

namespace MyGUI
{
	/// <summary>
	///		Do not directly use this class
	/// </summary>
	public class ListItem : PrimitiveComponent
	{
		public ListItem(string name, string value)
		{
			Name = name;
			Value = value;
			initRenderBuffer();
		}

		public Container<ListItem> Parent { get; set; }
		public Label Caption { get; set; }

		public string Value { get; set; }

		private Focus focusStatus;
		public override Focus FocusStatus
		{
			get => focusStatus;
			set
			{
				if (focusStatus != value)
				{
					focusStatus = value;
					UpdateRenderBuffer();
				}
			}
		}

		private Pixel[,] renderBuffer;
		private void initRenderBuffer()
		{
			renderBuffer = new Pixel[Width, Height];
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, j] = new Pixel();
				}
			}
			UpdateRenderBuffer();
			UpdateChunks.Add(new Chunk(new Coordinates(), Width, Height));
		}
		public override Pixel[,] GetRenderBuffer() => renderBuffer;

		public override void UpdateRenderBuffer()
		{
			ConsoleColor foregroundBrush;
			ConsoleColor backgroundBrush;
			switch (FocusStatus)
			{
				case Focus.Focusing:
					foregroundBrush = FocusingForegroundColor;
					backgroundBrush = FocusingBackgroundColor;
					break;

				case Focus.Focused:
					foregroundBrush = FocusedForegroundColor;
					backgroundBrush = FocusedBackgroundColor;
					break;

				case Focus.NoFocus:
					foregroundBrush = DefaultForegroundColor;
					backgroundBrush = DefaultBackgroundColor;
					break;
			}

			throw new NotImplementedException();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}
	}
}
