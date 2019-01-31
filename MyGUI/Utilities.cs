using System;
using System.Collections.Generic;
using System.Linq;
using CustomizedFunction;

namespace MyGUI.Utilities
{
	public struct Pixel
	{
		public Pixel(char displayCharacter, ConsoleColor foreground, ConsoleColor background)
		{
			DisplayCharacter = displayCharacter;
			ForegroundColor = foreground;
			BackgroundColor = background;
		}
		public Pixel(Pixel anotherPixel)
		{
			DisplayCharacter = anotherPixel.DisplayCharacter;
			ForegroundColor = anotherPixel.ForegroundColor;
			BackgroundColor = anotherPixel.BackgroundColor;
		}

		public char DisplayCharacter { get; set; }

		public ConsoleColor ForegroundColor { get; set; }

		public ConsoleColor BackgroundColor { get; set; }

		public bool Equals(Pixel another)
		{
			return
				DisplayCharacter == another.DisplayCharacter &&
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

	public interface IEntity : IKeyEvent, INameable, IFocusable
	{
		Coordinates Anchor { get; set; }
		int Width { get; set; }
		int Height { get; set; }

		Pixel[,] GetRenderBuffer();
	}

	public interface IKeyEvent
	{
		bool ParseAndExecute(ConsoleKeyInfo key);
	}

	public interface INameable
	{
		string Name { get; set; }
	}

	public interface IFocusable
	{
		Focus FocusStatus { get; set; }
	}

	public enum Focus
	{
		Focusing,
		Focused,
		NoFocus
	}

	public class EntityCollection<T> where T : IEntity
	{
		public EntityCollection(CustomFunctionBuilder more_AddElement_Behavior = null)
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
			element.Name = name == "" ? $"{element.GetType().ToString()}{collection.Count}" : name;
			collection.Add(element);

			customFunction?.Invoke();
		}

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
	}

	public abstract class Container<TItem> : IEntity where TItem : Component
	{
		public Coordinates Anchor { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public string Name { get; set; }
		public Focus FocusStatus { get; set; }
		public List<TItem> List { get; set; } = new List<TItem>();

		public Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}

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
	}
}