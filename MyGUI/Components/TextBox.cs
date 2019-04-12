using System;
using MyGUI.Utilities;
using static MyGUI.Session.Settings.Console;
using static MyGUI.Session.Settings.Appearance.ComponentStyle.BorderStyle;

namespace MyGUI
{
	public class TextBox : PrimitiveComponent, IValue<string>
	{
        public TextBox(int width, string name, string defaultString)
        {
            Height = 3;
            Width = width;
            Name = name;
            value = defaultString;
            OnValueChanged += s => UpdateRenderBuffer();
			DisplayAreaComponent = new DisplayArea(Width - 2, this);
			CursorComponent = new Cursor(DisplayAreaComponent);
            initRenderBuffer();
        }

		public readonly Point DisplayAreaAnchor = new Point() { X = 1, Y = 1 };

        public event Action<string> OnValueChanged;

		private string value;
        public string Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnValueChanged(value);
                }
            }
        }

        protected DisplayArea DisplayAreaComponent { get; set; }
		protected Cursor CursorComponent { get; set; }

        private Pixel[,] renderBuffer;
        private void initRenderBuffer()
        {
            renderBuffer = new Pixel[Width, Height];
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    renderBuffer[i, j] = new Pixel(' ', ForegroundBrush, BackgroundBrush);

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
			DisplayAreaComponent.UpdateRenderBuffer();
			Pixel[,] renderBufferRef = DisplayAreaComponent.GetRenderBuffer();
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, j].ForegroundColor = ForegroundBrush;
					renderBuffer[i, j].BackgroundColor = BackgroundBrush;
				}
			}
			for (int i = 0; i < DisplayAreaComponent.Width; i++)
			{
				Pixel p = new Pixel
				{
					Character = renderBufferRef[i, 0].Character,
					ForegroundColor = DefaultForegroundColor,
					BackgroundColor = DefaultBackgroundColor,
				};
				renderBuffer[i + DisplayAreaAnchor.X, DisplayAreaAnchor.Y] = p;
			}

			{
				Pixel c = CursorComponent.GetRenderBuffer()[0, 0];
				Pixel p = new Pixel
				{
					Character = renderBuffer[DisplayAreaAnchor.X + CursorComponent.Anchor.X, 1].Character,
					ForegroundColor = c.ForegroundColor,
					BackgroundColor = c.BackgroundColor
				};
				renderBuffer[DisplayAreaAnchor.X + CursorComponent.Anchor.X, 1] = p;
			}
		}

		protected void TryMoveCursorLeft()
		{
			DisplayAreaComponent.MoveLeft(CursorComponent.MoveLeft());
			UpdateRenderBuffer();
		}
		protected void TryMoveCursorRight()
		{
			DisplayAreaComponent.MoveRight(CursorComponent.MoveRight());
			UpdateRenderBuffer();
		}

		protected void Remove()
		{
			try
			{
				value = value.Remove(DisplayAreaComponent.Anchor.X + CursorComponent.Anchor.X - 1, 1);
				TryMoveCursorLeft();
				OnValueChanged(value);
			}
			catch (ArgumentOutOfRangeException) { }
		}

		protected void Delete()
		{
			try
			{
				value = value.Remove(DisplayAreaComponent.Anchor.X + CursorComponent.Anchor.X, 1);
				OnValueChanged(value);
			}
			catch (ArgumentOutOfRangeException) { }
		}

		protected void Write(char input)
		{
			value = value.Insert(DisplayAreaComponent.Anchor.X + CursorComponent.Anchor.X, input.ToString());
			TryMoveCursorRight();
			OnValueChanged(value);
		}

		protected string Submit()
		{
			return Value;
		}

		protected void SetCursorToFocus()
		{
			CursorComponent.FocusStatus = Focus.Focusing;
			FocusStatus = Focus.Focused;
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			bool result = false;
			if (CursorComponent.FocusStatus == Focus.Focusing)
			{
				result = CursorComponent.ParseAndExecute(key);
			}

			if (!result)
			{
				int ascii = key.KeyChar;
				if (ascii >= 32 && ascii <= 126)
				{
					Write(key.KeyChar);
				}
				else
				{
					switch (key.Key)
					{
						case ConsoleKey.Backspace:
							Remove();
							break;

						case ConsoleKey.Delete:
							Delete();
							break;

						case ConsoleKey.Enter:
							SetCursorToFocus();
							break;

						default:
							return false;
					}
				}
			}

			UpdateRenderBuffer();
			return true;
		}

        protected class DisplayArea
        {
			public DisplayArea(int width, TextBox parent)
			{
				Width = width;
				Anchor = new Point();
				this.parent = parent;
				CursorComponent = new Cursor(this);
				initRenderBuffer();
			}

			public Point Anchor;
			public readonly int Height = 1;
			public int Width { get; set; }

			public TextBox parent;
			public Cursor CursorComponent { get; set; }

			private Pixel[,] renderBuffer;
			public void initRenderBuffer()
			{
				renderBuffer = new Pixel[Width, Height];
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, 0] = new Pixel() { Character = ' ' };
				}

				UpdateRenderBuffer();
			}
			public Pixel[,] GetRenderBuffer()
			{
				return renderBuffer;
			}
			public void UpdateRenderBuffer()
			{
				for (int i = 0; i < Width; i++)
				{
					if (i + Anchor.X < parent.value.Length)
					{
						renderBuffer[i, 0].Character = parent.value[i + Anchor.X];
					}
					else
					{
						renderBuffer[i, 0].Character = ' ';
					}
				}
			}

			public void MoveLeft(bool isCursorMove)
			{
				if (!isCursorMove)
				{
					if (Anchor.X > 0)
					{
						Anchor.X--;
					}
				}
			}
			public void MoveRight(bool isCurMove)
			{
				if (!isCurMove)
				{
					if (Anchor.X < parent.value.Length - this.Width)
					{
						Anchor.X++;
					}
				}
			}
        }

		protected class Cursor : PrimitiveComponent
		{
			public Cursor(DisplayArea displayArea)
			{
				this.displayArea = displayArea;
				parent = displayArea.parent;
			}

			private DisplayArea displayArea;

			public bool MoveLeft()
			{
				if (Anchor.X <= 0)
				{
					return false;
				}
				else
				{
					Anchor.X--;
					return true;
				}
			}

			public bool MoveRight()
			{
				if (Anchor.X >= displayArea.parent.value.Length - 1 || Anchor.X >= displayArea.Width - 1)
				{
					return false;
				}
				else
				{
					Anchor.X++;
					return true;
				}
			}

			public override Pixel[,] GetRenderBuffer()
			{
				Pixel[,] pixel = new Pixel[1, 1];
				pixel[0, 0] = new Pixel() { ForegroundColor = ForegroundBrush, BackgroundColor = BackgroundBrush };
				return pixel;
			}

			public override bool ParseAndExecute(ConsoleKeyInfo key)
			{
				switch (key.Key)
				{
					case ConsoleKey.RightArrow:
						displayArea.MoveRight(MoveRight());
						break;

					case ConsoleKey.LeftArrow:
						displayArea.MoveLeft(MoveLeft());
						break;

					case ConsoleKey.Escape:
						FocusStatus = Focus.NoFocus;
						parent.FocusStatus = Focus.Focusing;
						break;

					default:
						return false;
				}

				return true;
			}

			public override void UpdateRenderBuffer()
			{
				
			}
		}

		protected struct MovementStatus
		{
			public MovementStatus(bool isMoveDisplayArea, bool isMoveCursor)
			{
				this.isMoveDisplayArea = isMoveDisplayArea;
				this.isMoveCursor = isMoveCursor;
			}

			public bool isMoveDisplayArea;
			public bool isMoveCursor;
		}
	}
}
