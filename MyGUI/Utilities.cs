using System;
using MyGUI.Utilities;
using System.Collections.Generic;

namespace MyGUI.Utilities
{
	public interface IEntity : IKeyEvent, INameable, IFocusable, IVisible, IDisposable
	{
		Point Anchor { get; set; }
		int Width { get; }
		int Height { get; }
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

	public interface IVisible
	{
		List<Point> UpdateChunks { get; set; }
		Pixel[,] GetRenderBuffer();
		void UpdateRenderBuffer();
	}

	public interface IValue<T>
	{
		T Value { get; set; }
		event Action<T> OnValueChanged;
	}

	public enum Focus
	{
		Focusing,
		Focused,
		NoFocus,
		Selected
	}
}

namespace MyGUI
{
	public class Resource
	{
		public static T GetComponent<T>(string name) where T : IEntity
		{
			return (T)Session.Resources.ActiveEntities[name];
		}
		public static T GetComponent<T>(int index) where T : IEntity
		{
			return (T)Session.Resources.ActiveEntities[index];
		}
	}
}