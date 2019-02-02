using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;
using System.Linq;
using static MyGUI.Session.Settings.Appearance.ComponentStyle;
using static MyGUI.Session.Settings.Console;

namespace MyGUI
{
	public class ListBox : Container<ListItem>
	{
		#region Methods of Instantiating ComboBox
		public ListBox(int height, int width, params MyGUI.ListItem[] items)
		{
			Height = height;
			Width = width;
			Collection.AddRange(items);
			Collection.SetFocusing(0);
			DisplayAreaComponent = new DisplayArea(Height - 2, Width - 2, this);
			initRenderBuffer();
		}
		public ListBox(int height, int width, params ListItem[] items)
		{
			Height = height;
			Width = width;
			Collection.AddRange(CreateListItem(items));
			Collection.SetFocusing(0);
			DisplayAreaComponent = new DisplayArea(Height - 2, Width - 2, this);
			initRenderBuffer();
		}
		public ListBox(params MyGUI.ListItem[] items) : this(DefaultHeight, DefaultWidth, items) { }
		public ListBox(params ListItem[] items) : this(DefaultHeight, DefaultWidth, items) { }
		public ListBox(int height , int width ) : this(height, width, new MyGUI.ListItem[0]) { }
		public ListBox() : this(DefaultHeight, DefaultWidth, new MyGUI.ListItem[0]){ }
		#endregion

		public const int DefaultHeight = MinHeight;
		public const int MaxHeight = 12;
		public const int MinHeight = 3;
		public const int DefaultWidth = MinWidth;
		public const int MaxWidth = 50;
		public const int MinWidth = 12;

		private int height;
		public override int Height
		{
			get => height;
			set
			{
				if (value > MaxHeight) height = MaxHeight;
				else if (value < MinHeight) height = MinHeight;
				else height = value;
			}
		}

		private int width;
		public override int Width
		{
			get => width;
			set
			{
				if (value > MaxWidth) width = MaxWidth;
				else if (value < MinWidth) width = MinWidth;
				else width = value;
			}
		}

		public bool IsAcceptTextInput { get; set; }
		public int CandidateNumber { get; set; } = 4;

		private DisplayArea DisplayAreaComponent { get; set; }

		private int currentItem;
		public int CurrentItem
		{
			get => currentItem;
			set
			{
				if (value >= Collection.Count) value = 0;
				else if (value < 0) value = Collection.Count - 1;
				else currentItem = value;
			}
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			if (!DisplayAreaComponent.ParseAndExecute(key))
			{
				switch (key.Key)
				{
					case ConsoleKey.Escape:
						Collection.SetFocusing();
						break;

					default:
						return false;
				}
			}

			return true;
		}

		#region RenderBuffer Related
		private Pixel[,] renderBuffer;
		private void initRenderBuffer()
		{
			renderBuffer = new Pixel[width, height];
			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					renderBuffer[i, j] = new Pixel() {
						ForegroundColor = DefaultForegroundColor,
						BackgroundColor = DefaultBackgroundColor
					};
					if      (j == 0      && i == 0)      renderBuffer[i, j].Character = BorderStyle.UpperLeft ;
					else if (j == height && i == 0)      renderBuffer[i, j].Character = BorderStyle.LowerLeft ;
					else if (j == 0      && i == width)  renderBuffer[i, j].Character = BorderStyle.UpperRight;
					else if (j == height && i == width)  renderBuffer[i, j].Character = BorderStyle.LowerRight;
					else if (j == 0      || j == height) renderBuffer[i, j].Character = BorderStyle.Horizontal;
					else if (i == 0      || i == width)  renderBuffer[i, j].Character = BorderStyle.Vertical  ;
				}
			}
		}
		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public override void UpdateRenderBuffer()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Methods of Instantiating ListItems
		public MyGUI.ListItem[] CreateListItem(params ListItem[] items)
			=> items.Select(i => new MyGUI.ListItem(i.Name, i.Value, Width - 2, this)).ToArray();

		// It is a wrapper of MyGUI.ListItem
		public class ListItem : INameable
		{
			public string Name { get; set; }
			public string Value { get; set; }

			public ListItem(string name, string value = null)
			{
				if (value == null)
				{
					Value = name;
				}
				else
				{
					Value = value;
				}

				Name = name;
			}
		}
		#endregion

		private class DisplayArea : AbstractDisplayArea, IValue<string>
		{
			public DisplayArea(int height, int width, ListBox parent) : base()
			{
				Height = height;
				Width = width;
				SetParent(parent);
				parent = GetParent<ListBox>();
				initRenderBuffer();
			}

			public List<Chunk> updateChunk = new List<Chunk>();
			public string Value { get ; set; }
			public event Action<string> OnValueChanged;
			private Pixel[,] renderBuffer;

			private ListBox UnboxedParent;
			private void selectUpperItem()
			{
				UnboxedParent.CurrentItem++;
				UnboxedParent.Collection.SetFocusing(UnboxedParent.CurrentItem);
			}

			private void selectLowerItem()
			{
				UnboxedParent.CurrentItem--;
				UnboxedParent.Collection.SetFocusing(UnboxedParent.CurrentItem);
			}

			private void SubmitValue()
			{

			}

			private void initRenderBuffer()
			{
				renderBuffer = new Pixel[Width, Height];
				for (int j = 0; j < Height; j++)
				{
					for (int i = 0; i < Width; i++)
					{
						renderBuffer[i, j] = new Pixel(' ', DefaultForegroundColor, DefaultBackgroundColor);
					}
				}
				updateChunk.Add(new Chunk(new Coordinates(1, 1), Width, Height));
			}
			public override Pixel[,] GetRenderBuffer() => renderBuffer;
			public override void UpdateRenderBuffer()
			{
				throw new NotImplementedException();
			}

			public override bool ParseAndExecute(ConsoleKeyInfo key)
			{
				switch (key.Key)
				{
					case ConsoleKey.UpArrow:
					case ConsoleKey.LeftArrow:
						selectUpperItem();
						break;

					case ConsoleKey.DownArrow:
					case ConsoleKey.RightArrow:
					case ConsoleKey.Tab:
						selectLowerItem();
						break;

					case ConsoleKey.Enter:
						break;

					default:
						return false;
				}

				return true;
			}
		}
	}

	/// <summary>
	///		Do not directly use this class
	/// </summary>
	public class ListItem : PrimitiveComponent
	{
		internal ListItem(string name, string value, int width, Container<ListItem> parent)
		{
			Name = name;
			Value = value;
			Parent = parent;
			Height = 1;
			Width = width;
			initRenderBuffer();
		}

		public Container<ListItem> Parent { get; set; }

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
			UpdateChunks.Add(new Chunk(new Coordinates(), Width, Height));
		}
		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public override void UpdateRenderBuffer()
		{

			throw new NotImplementedException();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}
	}
}
