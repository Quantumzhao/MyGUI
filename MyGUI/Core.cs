using System;
using System.Collections.Generic;
using System.Linq;
using CustomizedFunction;
using Newtonsoft.Json.Linq;
using System.IO;

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

	public class AbstractCollection<T> where T : INameable, IFocusable
	{
		public AbstractCollection(CustomFunctionBuilder more_AddElement_Behavior = null)
		{
			customFunction = more_AddElement_Behavior;
		}

		protected List<T> collection = new List<T>();

		private CustomFunctionBuilder customFunction;

		public T this[int index]
		{
			get
			{
				try
				{
					return collection[index];
				}
				catch (IndexOutOfRangeException e)
				{
					throw e;
				}
			}

			set
			{
				try
				{
					collection[index] = value;
				}
				catch (IndexOutOfRangeException e)
				{
					throw e;
				}
			}
		}

		public T this[string name]
		{
			get
			{
				return collection.Where(t => t.Name == name).Single();
			}

			set
			{
				for (int i = 0; i < collection.Count; i++)
				{
					if (collection[i].Name.Equals(name))
					{
						collection[i] = value;
					}
				}
			}
		}

		public int Count => collection.Count;

		public void Add(T element, string name = "")
		{
			if (element.Name == "")
			{
				element.Name = name == "" ? $"{element.GetType().ToString()}{collection.Count}" : name;
			}
			collection.Add(element);
			SetFocusing(collection.Count - 1);

			customFunction?.Invoke();
		}

		public void AddRange(IEnumerable<T> items) => collection.AddRange(items);

		public T GetFocusing()
		{
			return collection.Where(t => t.FocusStatus == Focus.Focusing).Single();
		}
		public void SetFocusing(int index)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				if (i == index)
				{
					collection[i].FocusStatus = Focus.Focusing;
				}
				else
				{
					collection[i].FocusStatus = Focus.NoFocus;
				}
			}
		}
		/// <summary>
		///		Set the only Focusing element from FOCUSING => FOCUSED
		/// </summary>
		public void SetFocusing()
		{
			GetFocusing().FocusStatus = Focus.Focused;
		}

		public void Remove(T element)
		{
			collection.Remove(element);
		}
	}

	public abstract class PrimitiveComponent : IEntity
	{
		public Point Anchor { get; set; } = new Point();

		public int Width { get; set; }
		public int Height { get; set; }

		public string Name { get; set; }

		public List<Point> UpdateChunks { get; set; }

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

		//public EntityCollection<Component> internalComponentCollection { get; set; }

		public virtual Focus FocusStatus { get; set; }

		public abstract Pixel[,] GetRenderBuffer();

		public virtual bool ParseAndExecute(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				default:
					return false;
			}
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public abstract void UpdateRenderBuffer();
	}

	public abstract class Container<TItem> : IEntity where TItem : INameable, IFocusable, IVisible
	{
		public Point Anchor { get; set; }
		public virtual int Width { get; set; }
		public virtual int Height { get; set; }
		public string Name { get; set; }
		public Focus FocusStatus { get; set; }
		public AbstractCollection<TItem> Collection { get; set; } = new AbstractCollection<TItem>();

		public abstract Pixel[,] GetRenderBuffer();

		public List<Point> UpdateChunks { get; set; }

		public abstract bool ParseAndExecute(ConsoleKeyInfo key);

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

		public void Dispose()
		{
			Session.Resources.ActiveEntities.Remove(this);
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
	}

	public abstract class AbstractDisplayArea : PrimitiveComponent
	{
		public AbstractDisplayArea() => FocusStatus = Focus.NoFocus;

		public virtual void MoveUp() { }
		public virtual void MoveDown() { }
		public virtual void MoveLeft() { }
		public virtual void MoveRight() { }
	}
}