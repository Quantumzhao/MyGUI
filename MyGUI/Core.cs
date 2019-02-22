using System;
using System.IO;
using System.Linq;
using MyGUI.Session;
using CustomizedFunction;
using Newtonsoft.Json.Linq;
using SConsole = System.Console;
using System.Collections.Generic;
using MyConsole = MyGUI.Session.Console;
using static MyGUI.Session.Settings.Console;
using System.Collections;

namespace MyGUI.Utilities
{
	public struct Pixel
	{
		public Pixel(char displayCharacter, ConsoleColor foreground, ConsoleColor background)
		{
			Character = displayCharacter;
			ForegroundColor = foreground;
			BackgroundColor = background;
		}
		public Pixel(Pixel anotherPixel)
		{
			Character = anotherPixel.Character;
			ForegroundColor = anotherPixel.ForegroundColor;
			BackgroundColor = anotherPixel.BackgroundColor;
		}

		public char Character { get; set; }

		public ConsoleColor ForegroundColor { get; set; }

		public ConsoleColor BackgroundColor { get; set; }

		public bool Equals(Pixel another)
		{
			return
				Character == another.Character &&
				ForegroundColor == another.ForegroundColor &&
				BackgroundColor == another.BackgroundColor;
		}
	}

	public struct Point
	{
		public Point(int x = 0, int y = 0)
		{
			X = x;
			Y = y;
		}

		public int X;
		public int Y;

		public static bool operator ==(Point first, Point second)
		{
			if (second.X != first.X) return false;
			else if (second.Y != first.Y) return false;
			else return true;
		}

		public static bool operator !=(Point first, Point second)
		{
			if (second.X == first.X) return false;
			else if (second.Y == first.Y) return false;
			else return true;
		}

		public override bool Equals(object obj) => base.Equals(obj);

		public override int GetHashCode() => base.GetHashCode();
	}

	public class AbstractCollection<T> : List<T> where T : INameable, IFocusable
	{
		public AbstractCollection(CustomFunctionBuilder more_AddElement_Behavior = null)
		{
			customFunction = more_AddElement_Behavior;
		}

		private CustomFunctionBuilder customFunction;

		public T this[string name]
		{
			get
			{
				return this.Where(t => t.Name == name).Single();
			}

			set
			{
				for (int i = 0; i < Count; i++)
				{
					if (this[i].Name.Equals(name))
					{
						this[i] = value;
					}
				}
			}
		}

		private int pointer;
		public int Pointer
		{
			get => pointer;
			set
			{
				if (value < 0) pointer = Count - 1;
				else if (value >= Count) pointer = 0;
				else pointer = value;
			}
		}

		public void Add(T element, string name = "")
		{
			if (element.Name == "")
			{
				element.Name = name == "" ? $"{element.GetType().ToString()}{Count}" : name;
			}
			Add(element);
			SetFocusStatus(Count - 1, Focus.Focusing);

			customFunction?.Invoke();
		}

		public T Get(Focus focusStatus)
		{
			try
			{
				return this.Where(t => t.FocusStatus == focusStatus).Single();
			}
			catch (InvalidOperationException)
			{
				return default(T);
			}
		}
		public void SetFocusStatus(int index, Focus focusStatus)
		{
			for (int i = 0; i < Count; i++)
			{
				if (i == index)
				{
					this[i].FocusStatus = focusStatus;
				}
				else
				{
					this[i].FocusStatus = Focus.NoFocus;
				}
			}
		}
		/// <summary>
		///		Set the only Focusing element from FOCUSING => FOCUSED
		/// </summary>
		public void SetFocusing()
		{
			Get(Focus.Focusing).FocusStatus = Focus.Focused;
		}
		public void RemoveAllFocus()
		{
			foreach (var item in this)
			{
				item.FocusStatus = Focus.NoFocus;
			}
		}
	}

	public abstract class PrimitiveComponent : IEntity
	{
		public Point Anchor { get; set; } = new Point();

		public virtual int Width { get; set; }
		public virtual int Height { get; set; }

		public string Name { get; set; }

		public List<Point> UpdateChunks { get; set; } = new List<Point>();

		protected IEntity parent = null;
		public T GetParent<T>()
		{
			if (parent is T)
			{
				return (T)parent;
			}
			else
			{
				throw new InvalidCastException("Type parameter does not derive from IEntity");
			}
		}
		public void SetParent(IEntity target)
		{
			if (parent == null)
			{
				parent = target;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		private Focus focusStatus = Focus.NoFocus;
		public virtual Focus FocusStatus
		{
			get => focusStatus;
			set
			{
				if (value == Focus.Selected) value = Focus.Focusing;
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
					}
					UpdateRenderBuffer();
				}
			}
		}

		protected ConsoleColor ForegroundBrush = DefaultForegroundColor;
		protected ConsoleColor BackgroundBrush = DefaultBackgroundColor;

		public abstract Pixel[,] GetRenderBuffer();

		public virtual bool ParseAndExecute(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
				case ConsoleKey.LeftArrow:
					FocusStatus = Focus.NoFocus;
					break;

				case ConsoleKey.DownArrow:
				case ConsoleKey.RightArrow:
					FocusStatus = Focus.NoFocus;
					break;

				case ConsoleKey.Escape:
					FocusStatus = Focus.NoFocus;
					break;

				default:
					break;
			}

			return false;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public abstract void UpdateRenderBuffer();
	}

	public abstract class Container<TItem> : IEntity where TItem : IEntity
	{
		public Point Anchor { get; set; }
		public virtual int Width { get; set; }
		public virtual int Height { get; set; }
		public string Name { get; set; }
		public Focus FocusStatus { get; set; }
		public AbstractCollection<TItem> Collection { get; set; } = new AbstractCollection<TItem>();

		public abstract Pixel[,] GetRenderBuffer();

		public List<Point> UpdateChunks { get; set; }

		private IEntity parent;
		public T GetParent<T>()
		{
			if (parent is T)
			{
				return (T)parent;
			}
			else
			{
				throw new InvalidCastException("Type parameter does not derive from IEntity");
			}
		}
		public void SetParent(IEntity target)
		{
			if (parent == null)
			{
				parent = target;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		public virtual bool ParseAndExecute(ConsoleKeyInfo key)
		{
			var entity = Collection.Get(Focus.Focusing);
			if (entity == null || !entity.ParseAndExecute(key))
			{
				switch (key.Key)
				{
					case ConsoleKey.UpArrow:
					case ConsoleKey.LeftArrow:
						SelectEntity(-1);
						break;

					case ConsoleKey.DownArrow:
					case ConsoleKey.RightArrow:
						SelectEntity(1);
						break;

					case ConsoleKey.Enter:
						Collection.SetFocusStatus(Collection.Pointer, Focus.Focusing);
						break;

					case ConsoleKey.Escape:
						Resources.ActiveEntities.RemoveAllFocus();
						return false;

					default:
						break;
				}
			}

			return true;
		}
		private void SelectEntity(int posReletiveToPointer)
		{
			Collection.Pointer += posReletiveToPointer;
			Collection.SetFocusStatus(Collection.Pointer, Focus.Selected);
		}

		public void Dispose()
		{
			Resources.ActiveEntities.Remove(this);
		}

		public abstract void UpdateRenderBuffer();
	}

	class JsonHelper : IDisposable
	{
		string filepath;
		JObject jObject;

		public JsonHelper(string path)
		{
			filepath = path;
			using (StreamReader reader = new StreamReader(filepath))
			{
				string json = reader.ReadToEnd();
				jObject = JObject.Parse(json);
			}
		}

		public void Dispose() => File.WriteAllText(filepath, jObject.ToString());

		public JToken GetProperty(string name) => jObject.Property(name).Value;

		public void SetProperty<T>(string name, JToken value) => jObject.Property(name).Value = value;

		public void AddProperty(string name, JToken value) => jObject.Add(name, value);

		public IEnumerable<JProperty> ListProperties() => jObject.Properties();

		public bool Has(string propertyName) => jObject.Properties().Where(p => p.Name == propertyName).Count() != 0;
	}

	public static class Misc
	{
		public static void RemoveDuplicate(this List<Point> points)
		{
			for (int i = 0; i < points.Count; i++)
				for (int j = 0; j < i; j++)
					if (points[i] == points[j]) points.RemoveAt(i--);
		}

		public static void Render(this IEntity entity)
		{
			var ps = entity.GetRenderBuffer();
			for (int j = 0; j < ps.GetLength(1); j++)
			{
				for (int i = 0; i < ps.GetLength(0); i++)
				{
					SConsole.SetCursorPosition(i + entity.Anchor.X, j + entity.Anchor.Y);
					SConsole.ForegroundColor = ps[i, j].ForegroundColor;
					SConsole.BackgroundColor = ps[i, j].BackgroundColor;
					SConsole.Write(ps[i, j].Character);
				}
			}

			SConsole.CursorVisible = true;
			SConsole.ForegroundColor = Resources.DefaultSystemConsoleForegroundColor;
			SConsole.BackgroundColor = Resources.DefaultSystemConsoleBackgroundColor;
			SConsole.SetCursorPosition(0, MyConsole.tempAnchor.Y);
		}
	}

	public abstract class AbstractDisplayArea : PrimitiveComponent
	{
		public AbstractDisplayArea() => FocusStatus = Focus.NoFocus;

		public virtual void MoveUp() { }
		public virtual void MoveDown() { }
		public virtual void MoveLeft() { }
		public virtual void MoveRight() { }
	}

	public abstract class PrimitiveComponentWithValue<T> : PrimitiveComponent, IValue<T>
	{
		public abstract T Value { get; set; }

		public virtual event Action<T> OnValueChanged;
	}

	public abstract class Switch : PrimitiveComponentWithValue<bool>
	{
		public const int MinHeight = 1;
		public const int MinWidth = 6;
		public const int DefaultHeight = MinHeight;
		public const int DefaultWidth = 10;

		public virtual char Checked { get; protected set; }
		public virtual char Unchecked { get; protected set; }

		public Label LabelComponent { get; set; }
	}
}