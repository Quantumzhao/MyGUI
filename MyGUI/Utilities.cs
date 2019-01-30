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
		Focus IsFocused { get; set; }
	}

	public enum Focus
	{
		Focusing,
		Focused,
		NoFocus
	}

	public class AbstractCollection<T> where T : INameable
	{
		protected List<T> collection = new List<T>();

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

		public void Add(T element, string name = "", CustomFunctionBuilder customFunction = null)
		{
			element.Name = name == "" ? $"{element.GetType().ToString()}{collection.Count}" : name;
			collection.Add(element);

			customFunction?.Invoke();
		}
	}

	public abstract class Component
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
		public Focus IsFocused { get; set; }

		//private Pixel[,] renderBuffer;
		public virtual Pixel[,] GetRenderBuffer()
		{
			return new Pixel[Width, Height];
		}

		public abstract bool ParseAndExecute(ConsoleKeyInfo key);
	}
}