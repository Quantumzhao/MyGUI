using System;
using MyGUI.Utilities;

namespace MyGUI
{
	public class Label : PrimitiveComponent, IValue<string>
	{
		public Label(int height, int width, IEntity parent, string caption, string name = null)
		{
			Height = height;
			Width = width;
			SetParent(parent);
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

		private string value;
		public string Value
		{
			get => value;
			set
			{
				if (value != this.value)
				{
					this.value = value;
					OnValueChanged?.Invoke(value);
				}
			}
		}
		public bool IsWordWrap { get; set; } = false;

		private Pixel[,] renderBuffer;

		public event Action<string> OnValueChanged = null;

		public override Pixel[,] GetRenderBuffer()
		{
			return renderBuffer;
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
