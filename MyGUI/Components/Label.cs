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
	public class Label : PrimitiveComponent
	{
		public Label(int height, int width, string caption = null)
		{
			Height = height;
			Width = width;

			Caption = caption != null ? caption : Name;
			initRenderBuffer();
		}
		public Label(string caption = null) : this(DefaultHeight, DefaultWidth, caption) { }

		public const int DefaultHeight = 1;
		public const int DefaultWidth = 10;
		public string Caption { get; set; }
		public bool IsWordWrap { get; set; } = false;

		private Pixel[,] renderBuffer;
		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}
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
		}
		public override void UpdateRenderBuffer()
		{
			int charPtr = 0;
			int renderHeight = IsWordWrap ? Height : 1;
			for (int j = 0; j < renderHeight; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					if (charPtr < Caption.Length)
					{
						renderBuffer[i, 0].Character = Caption[charPtr];
						charPtr++;
					}
					else
					{
						if (j > 0) UpdateChunks.Add(new Chunk(new Coordinates(), Width, j - 1));
						UpdateChunks.Add(new Chunk(new Coordinates(0, j), i, 1));
						break;
					}
				}
			}
			if (!IsWordWrap && Caption.Length > Width)
			{
				for (int i = Width - 3; i < Width; i++)
				{
					renderBuffer[i, 0].Character = '.';
				}
			}
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				default:
					return false;
			}

#pragma warning disable CS0162 // Unreachable code detected
			return true;
#pragma warning restore CS0162 // Unreachable code detected
		}
	}
}
