using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;
using System.Linq;
using static MyGUI.Session.Settings.Appearance.ComponentStyle;

namespace MyGUI
{
	public class ListBox : Container<ListItem>
	{
		#region Methods of Instantiating ComboBox
		public ListBox(int height, int width, params MyGUI.ListItem[] items)
		{
			Height = height;
			Width = width;
			Collection.AddRange(items.Select(i => { i.Parent = this; return i; }));
			Collection.SetFocusing(0);
			initRenderBuffer();
		}
		public ListBox(int height, int width, params ListItem[] items) : this(height,width, CreateListItem()) { }
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
			switch (key.Key)
			{
				case ConsoleKey.LeftArrow:
				case ConsoleKey.UpArrow:
					selectUpperItem();
					break;

				case ConsoleKey.Tab:
				case ConsoleKey.RightArrow:
				case ConsoleKey.DownArrow:
					selectLowerItem();
					break;

				case ConsoleKey.Escape:
					Collection.SetFocusing();
					break;

				default:
					return false;
			}

			return true;
		}

		private void selectUpperItem()
		{
			CurrentItem++;
			Collection.SetFocusing(CurrentItem);
		}

		private void selectLowerItem()
		{
			CurrentItem--;
			Collection.SetFocusing(CurrentItem);
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
						ForegroundColor = ConsoleColor.White,
						BackgroundColor = ConsoleColor.Black
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
		#endregion
		#region Methods of Instantiating ListItems
		public static MyGUI.ListItem[] CreateListItem(params ListItem[] items)
			=> items.Select(i => new MyGUI.ListItem(i.Name, i.Value)).ToArray();

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

		private class ListBoxDisplayArea : DisplayArea
		{
			public override Pixel[,] GetRenderBuffer()
			{
				throw new NotImplementedException();
			}

			public override bool ParseAndExecute(ConsoleKeyInfo key)
			{
				throw new NotImplementedException();
			}
		}
	}

	/// <summary>
	///		Do not directly use this class
	/// </summary>
	public class ListItem : INameable, IFocusable, IVisible
	{
		internal ListItem(string name, string value, Container<ListItem> parent = null)
		{
			Name = name;
			Value = value;
			Parent = parent;
		}

		public Container<ListItem> Parent { get; set; }

		public Coordinates Anchor { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public Focus FocusStatus { get; set; } = Focus.Focusing;

		public Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}

	}
}
