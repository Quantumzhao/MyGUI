using System;
using System.Linq;
using MyGUI.Utilities;
using static MyGUI.Session.Settings.Console;

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

		//private Focus focusStatus = Focus.NoFocus;
		//public override Focus FocusStatus
		//{
		//	get => focusStatus;
		//	set
		//	{
		//		if (focusStatus != value)
		//		{
		//			focusStatus = value;
		//			switch (value)
		//			{
		//				case Focus.Focusing:
		//					ForegroundBrush = FocusingForegroundColor;
		//					BackgroundBrush = FocusingBackgroundColor;
		//					break;
		//				case Focus.Focused:
		//					ForegroundBrush = FocusedForegroundColor;
		//					BackgroundBrush = FocusedBackgroundColor;
		//					break;
		//				case Focus.NoFocus:
		//					ForegroundBrush = DefaultForegroundColor;
		//					BackgroundBrush = DefaultBackgroundColor;
		//					break;
		//				case Focus.Selected:
		//					ForegroundBrush = SelectedForegroundColor;
		//					BackgroundBrush = SelectedBackgroundColor;
		//					break;
		//			}
		//			UpdateRenderBuffer();
		//		}
		//	}
		//}

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
					renderBuffer[i + x, j + y].ForegroundColor = ForegroundBrush;
					renderBuffer[i + x, j + y].BackgroundColor = BackgroundBrush;
				}
			}
			UpdateChunks.AddRange(LabelComponent.UpdateChunks.Select(p => new Point(p.X + x, p.Y + y)));

			renderBuffer[0, 0].Character = value ? Checked : Unchecked;
			UpdateChunks.Add(new Point(0, 0));
			UpdateChunks.RemoveDuplicate();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			if (!base.ParseAndExecute(key))
			{
				switch (key.Key)
				{
					case ConsoleKey.Enter:
						Value = !Value;
						break;

					default:
						return false;
				}
			}
			return true;
		}
	}
}
