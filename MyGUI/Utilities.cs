using System;
using System.Collections.Generic;
using System.Linq;
using CustomizedFunction;

namespace MyGUI.Utilities
{
	public interface IEntity : IKeyEvent, INameable, IFocusable, IVisible
	{
		Coordinates Anchor { get; set; }
		int Width { get; set; }
		int Height { get; set; }
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
		Pixel[,] GetRenderBuffer();
	}

	public enum Focus
	{
		Focusing,
		Focused,
		NoFocus
	}

}