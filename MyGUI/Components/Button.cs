using System;
using MyGUI.Utilities;
using System.Threading;
using static MyGUI.Session.Settings.Console;
using static MyGUI.Session.Settings.Appearance.ComponentStyle.SeparatorStyle;

namespace MyGUI
{
	public class Button : PrimitiveComponent
	{
		public Button(int height, int width, string name, string caption, Action onClick = null)
		{
			Height = height;
			Width = width;
			Name = name;
			LabelComponent = new Label(1, Width - 2, this, caption, caption);
			LabelComponent.Anchor = new Point(1, 1);
			OnClick += OnClickAnimation;
			OnClick += onClick;
			initRenderBuffer();
		}

		public const int MinHeight = 3;
		public const int MinWidth = 6;
		public const int DefaultHeight = MinHeight;

		public event Action OnClick;
		public Label LabelComponent { get; set; }

		private Pixel[,] renderBuffer;
		private void initRenderBuffer()
		{
			renderBuffer = new Pixel[Width, Height];
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, j] = new Pixel(' ',ForegroundBrush, BackgroundBrush);

					if (j == 0 || j == Height - 1)
					{
						renderBuffer[i, j].Character = Horizontal;
					}
					else if (i == 0 || i == Width - 1)
					{
						renderBuffer[i, j].Character = Vertical;
					}
				}
			}
			renderBuffer[0, 0].Character = UpperLeft;
			renderBuffer[Width - 1, 0].Character = UpperRight;
			renderBuffer[0, Height - 1].Character = LowerLeft;
			renderBuffer[Width - 1, Height - 1].Character = LowerRight;
			UpdateRenderBuffer();
		}
		public override Pixel[,] GetRenderBuffer()
		{
			return renderBuffer;
		}
		public override void UpdateRenderBuffer()
		{
			Pixel[,] renderBufferRef = LabelComponent.GetRenderBuffer();
			int x = LabelComponent.Anchor.X;
			int y = LabelComponent.Anchor.Y;
			for (int j = 0; j < LabelComponent.Height; j++)
			{
				for (int i = 0; i < LabelComponent.Width; i++)
				{
					renderBuffer[i + x, j + y].Character = renderBufferRef[i, j].Character;
				}
			}
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, j].ForegroundColor = ForegroundBrush;
					renderBuffer[i, j].BackgroundColor = BackgroundBrush;
				}
			}
		}

		public void OnClickAnimation()
		{
			ForegroundBrush = ConsoleColor.White;
			BackgroundBrush = ConsoleColor.Cyan;
			UpdateRenderBuffer();
			this.Render();
			Thread.Sleep(100);
			ForegroundBrush = FocusingForegroundColor;
			BackgroundBrush = FocusingBackgroundColor;
			UpdateRenderBuffer();
			this.Render();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			if (!base.ParseAndExecute(key))
			{
				switch (key.Key)
				{
					case ConsoleKey.Enter:
						OnClick?.Invoke();
						break;

					default:
						return false;
				}
			}
			return true;
		}

	}
}
