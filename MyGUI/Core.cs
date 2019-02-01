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

	public class Coordinates
	{
		public Coordinates(int x = 0, int y = 0)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }
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
	}

	public abstract class Component : IEntity
	{
		public Coordinates Anchor { get; set; } = new Coordinates();

		public int Width { get; set; }
		public int Height { get; set; }

		public string Name { get; set; }

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

		//public EntityCollection<Component> internalComponentCollection { get; set; }

		public Focus FocusStatus { get; set; }

		public abstract Pixel[,] GetRenderBuffer();

		public abstract bool ParseAndExecute(ConsoleKeyInfo key);

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}

	public abstract class Container<TItem> : IEntity where TItem : INameable, IFocusable, IVisible
	{
		public Coordinates Anchor { get; set; }
		public virtual int Width { get; set; }
		public virtual int Height { get; set; }
		public string Name { get; set; }
		public Focus FocusStatus { get; set; }
		public AbstractCollection<TItem> Collection { get; set; } = new AbstractCollection<TItem>();

		public abstract Pixel[,] GetRenderBuffer();

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
			throw new NotImplementedException();
		}
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

	public abstract class DisplayArea : Component
	{
		public virtual void MoveUp() { }
		public virtual void MoveDown() { }
		public virtual void MoveLeft() { }
		public virtual void MoveRight() { }
	}
}