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
	class CheckBox : PrimitiveComponent, IValue<bool>
	{
		public CheckBox(int height, int width, bool isChecked, string caption, string name = null)
		{
			Height = height;
			Width = width;
			Value = isChecked;

			LabelComponent = new Label(height, width - 2, this, caption, name ?? caption);

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
		public override Pixel[,] GetRenderBuffer()
		{
			return renderBuffer;
		}
		public override void UpdateRenderBuffer()
		{


			renderBuffer[0, 0].Character = value ? Checked : Unchecked;
			UpdateChunks.Add(new Point(0, 0));
		}
	}
}
