﻿using System;
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
	public class Label : PrimitiveComponent, IValue<string>
	{
		public Label(int height, int width, IEntity parent, string caption, string name = null)
		{
			Height = height;
			Width = width;
			Value = caption;
			Name = name ?? caption;
			initRenderBuffer();
		}
		public Label(string caption, IEntity parent = null)
			: this(DefaultHeight, DefaultWidth, parent, caption, caption) { }

		public const int MinHeight = 1;
		public const int MinWidth = 4;
		public const int DefaultHeight = MinHeight;
		public const int DefaultWidth = 10;

		public override int Height
		{
			get => base.Height;
			set
			{
				if (value < MinHeight) base.Height = MinHeight;
				else base.Height = value;
			}
		}

		public override int Width
		{
			get => base.Width;
			set
			{
				if (value < MinWidth) base.Width = MinWidth;
				else base.Width = value;
			}
		}

		public string Value { get; set; }
		public bool IsWordWrap { get; set; } = false;

		private Pixel[,] renderBuffer;

		public event Action<string> OnValueChanged = null;

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
					if (charPtr < Value.Length)
					{
						if (renderBuffer[i, 0].Character != Value[charPtr])
						{
							renderBuffer[i, 0].Character = Value[charPtr];
							UpdateChunks.Add(new Point(i, j));
						}
						charPtr++;
					}
					else
					{
						break;
					}
				}
			}
			if (!IsWordWrap && Value.Length > Width)
			{
				for (int i = Width - 3; i < Width; i++)
				{
					renderBuffer[i, 0].Character = '.';
				}
			}
			parent.UpdateChunks.AddRange(UpdateChunks);
		}
	}
}
