﻿using System;
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
	///		<para>Do not directly use this class</para>
	///		<para>Use it like this: <c>new ListBox(new ListItem())</c></para>
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

				default:
					foregroundBrush = DefaultForegroundColor;
					backgroundBrush = DefaultBackgroundColor;
					break;
			}

			Pixel[,] buffer = Caption.GetRenderBuffer();
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, j].ForegroundColor = foregroundBrush;
					renderBuffer[i, j].BackgroundColor = backgroundBrush;
					renderBuffer[i, j].Character = buffer[i, j].Character;
					UpdateChunks.Add(new Point(i, j));
				}
			}

			UpdateChunks.AddRange(Caption.UpdateChunks);
			UpdateChunks.RemoveDuplicate();
		}
	}
}
