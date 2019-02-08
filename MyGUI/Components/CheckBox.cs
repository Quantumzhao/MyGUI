using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUI.Session;
using MyGUI.Utilities;
using Newtonsoft.Json.Linq;
using static MyGUI.Session.Settings.Appearance.ComponentStyle;
using static MyGUI.Session.Settings.Appearance.ComponentStyle.SeparatorStyle;
using static MyGUI.Session.Settings.Console;
using static MyGUI.Session.Console;
using static MyGUI.Session.Resources;
using CustomizedFunction;
using System.IO;

namespace MyGUI
{
	public class CheckBox : PrimitiveComponent, IValue<bool>
	{
		public CheckBox(int height, int width, bool isChecked, string caption, string name = null)
		{
			Height = height;
			Width = width;
			Value = isChecked;

			LabelComponent = new Label(height, width - 2, this, caption, name ?? caption);
			LabelComponent.Anchor = new Point(2, 0);

			Name = name;
			initRenderBuffer();
		}
		public CheckBox(string caption) : this(DefaultHeight, DefaultWidth, false, caption, caption) { }

		public const int MinHeight = 1;
		public const int MinWidth = 6;
		public const int DefaultHeight = MinHeight;
		public const int DefaultWidth = 10;

		public readonly char Checked = '■';
		public readonly char Unchecked = '□';

		public Label LabelComponent { get; set; }

		private bool value;
		public bool Value
		{
			get => value;
			set
			{
				if (value != this.value)
				{
					this.value = value;
					OnValueChanged?.Invoke(value);
					UpdateRenderBuffer();
				}
			}
		}

		public event Action<bool> OnValueChanged;

		private Focus focusStatus = Focus.NoFocus;
		public override Focus FocusStatus
		{
			get => focusStatus;
			set
			{
				if (focusStatus != value)
				{
					focusStatus = value;
					switch (value)
					{
						case Focus.Focusing:
							ForegroundBrush = FocusingForegroundColor;
							BackgroundBrush = FocusingBackgroundColor;
							break;
						case Focus.Focused:
							ForegroundBrush = FocusedForegroundColor;
							BackgroundBrush = FocusedBackgroundColor;
							break;
						case Focus.NoFocus:
							ForegroundBrush = DefaultForegroundColor;
							BackgroundBrush = DefaultBackgroundColor;
							break;
						case Focus.Selected:
							ForegroundBrush = SelectedForegroundColor;
							BackgroundBrush = SelectedBackgroundColor;
							break;
					}
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
					renderBuffer[i, j] = new Pixel(' ', ForegroundBrush, BackgroundBrush);
				}
			}
			UpdateRenderBuffer();
		}
		public override Pixel[,] GetRenderBuffer()
		{
			return renderBuffer;
		}
		public override void UpdateRenderBuffer()
		{
			var renderBufferRef = LabelComponent.GetRenderBuffer();
			var x = LabelComponent.Anchor.X;
			var y = LabelComponent.Anchor.Y;
			for (int j = 0; j < LabelComponent.Height; j++)
			{
				for (int i = 0; i < LabelComponent.Width; i++)
				{
					renderBuffer[i + x, j + y].Character = renderBufferRef[i, j].Character;
				}
			}
			UpdateChunks.AddRange(LabelComponent.UpdateChunks.Select(p => new Point(p.X + x, p.Y + y)));

			renderBuffer[0, 0].Character = value ? Checked : Unchecked;
			UpdateChunks.Add(new Point(0, 0));
			UpdateChunks.RemoveDuplicate();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Enter:
					value = !value;
					break;

				case ConsoleKey.Escape:
					FocusStatus = Focus.NoFocus;
					break;

				default:
					return false;
			}

			return true;
		}
	}
}
