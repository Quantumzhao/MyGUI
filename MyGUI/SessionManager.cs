﻿using System;
using System.Text;
using System.Linq;
using System.Drawing;
using MyGUI.Utilities;
using SConsole = System.Console;
using System.Collections.Generic;
using MyPoint = MyGUI.Utilities.Point;

namespace MyGUI.Session
{
	public static class Resources
	{
		static Resources()
		{
			Settings.isInitialized = true;
		}

		// This filed is used to trigger the static constructor
		internal static bool isInitialized = false;

		internal static IOutput Output = new ConsoleOutput();
		internal static ConsoleColor DefaultSystemConsoleBackgroundColor = Output.BackgroundColor;
		internal static ConsoleColor DefaultSystemConsoleForegroundColor = Output.ForegroundColor;
		internal static MyPoint SystemConsoleCursorPosition = new MyPoint(Output.CursorLeft, Output.CursorTop);

		internal static AbstractCollection<IEntity> ActiveEntities { get; set; } = new AbstractCollection<IEntity>();

		public static string ReturnValueCache { get; internal set; } = null;
		public static IEnumerable<string> ReturnValueListCache { get; internal set; } = null;

		static T GetComponent<T>(string name) where T : IEntity
		{
			return (T)ActiveEntities[name];
		}
		static T GetComponent<T>(int index) where T : IEntity
		{
			return (T)ActiveEntities[index];
		}
	}

	public static class Settings
	{
		private static JsonHelper jsonHelper = null;
		static Settings()
		{
			try
			{
				jsonHelper = new JsonHelper("config.json");
			}
			catch
			{

			}
		}

		// This filed is used to trigger the static constructor
		public static bool isInitialized = false;

		public static class Appearance
		{
			public static class ComponentStyle
			{
				public static class BorderStyle
				{
					public static readonly char UpperLeft = '╔';
					public static readonly char LowerLeft = '╚';
					public static readonly char UpperRight = '╗';
					public static readonly char LowerRight = '╝';
					public static readonly char Horizontal = '═';
					public static readonly char Vertical = '║';
					public static readonly char T = '╦';
					public static readonly char T_UpSideDown = '╩';
					public static readonly char T_AntiClockwise90 = '╠';
					public static readonly char T_Clockwise90 = '╣';
					public static readonly char Cross = '╬';
				}

				public static class SeparatorStyle
				{
					public static readonly char UpperLeft = '┌';
					public static readonly char LowerLeft = '└';
					public static readonly char UpperRight = '┐';
					public static readonly char LowerRight = '┘';
					public static readonly char Horizontal = '─';
					public static readonly char Vertical = '│';
					public static readonly char T = '┬';
					public static readonly char T_UpSideDown = '┴';
					public static readonly char T_AntiClockwise90 = '├';
					public static readonly char T_Clockwise90 = '┤';
					public static readonly char Cross = '┼';
				}
			}
		}

		public static class Console
		{
			public static readonly Color[] ColorPalette = new Color[16];
			public static readonly ConsoleColor DefaultForegroundColor = ConsoleColor.White;
			public static readonly ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
			public static readonly ConsoleColor FocusingForegroundColor = ConsoleColor.White;
			public static readonly ConsoleColor FocusingBackgroundColor = ConsoleColor.Blue;
			public static readonly ConsoleColor FocusedForegroundColor = ConsoleColor.Black;
			public static readonly ConsoleColor FocusedBackgroundColor = ConsoleColor.Gray;
			public static readonly ConsoleColor SelectedForegroundColor = ConsoleColor.Black;
			public static readonly ConsoleColor SelectedBackgroundColor = ConsoleColor.Gray;
		}
	}

	public static class Console
	{
		static Console()
		{
			Resources.isInitialized = true;
			tempAnchor = new MyPoint(0,Resources.Output.CursorTop + 1);
		}

		internal static MyPoint tempAnchor;
		private static List<MyPoint> updateChunk = new List<MyPoint>();

		private static bool isExit = false;

		// Uncapsulated version of "Prompt"
		public static void Load(PrimitiveComponent component)
		{

		}
		public static void Activate(string entityName)
		{

		}

		public static void ExitInteraction() => isExit = true;

		public static void Prompt(params IEntity[] entities)
		{
			Resources.ActiveEntities.AddRange(
				entities.Select(e =>
				{
					e.Anchor = tempAnchor;
					tempAnchor = new MyPoint(tempAnchor.X, tempAnchor.Y + e.Height);
					return e;
				})
			);
			Resources.SystemConsoleCursorPosition = tempAnchor;
			Resources.ActiveEntities.SetFocusStatus(0, Focus.Selected);

			Resources.Output.OutputEncoding = Encoding.UTF8;
			Renderer.Render();
		}
		public static void Prompt(MyPoint anchor, IEntity entity)
		{

		}

		public static void Execute(string command = null)
		{
			initialize();

			if (command == null) Main();
			else Main(command);

			finalize();
		}
		private static void initialize()
		{
			Resources.Output.CursorVisible = false;
		}
		private static void finalize()
		{
			Resources.Output.CursorVisible = true;
			Resources.Output.ForegroundColor = Resources.DefaultSystemConsoleForegroundColor;
			Resources.Output.BackgroundColor = Resources.DefaultSystemConsoleBackgroundColor;
			Resources.Output.SetCursorPosition(0, tempAnchor.Y);
		}

		private static void Main()
		{
			while (ExecuteConsoleKey()) Renderer.Render();
			Renderer.Render();
		}
		private static void Main(string command)
		{
			throw new NotImplementedException();
		}

		private static void SelectEntity(int posReletiveToPointer)
		{
			var e = Resources.ActiveEntities;
			e.SetFocusStatus(e.Pointer, Focus.NoFocus);
			e.Pointer += posReletiveToPointer;
			e.SetFocusStatus(e.Pointer, Focus.Selected);
		}

		public static string GetUserInput()
		{
			return Resources.ReturnValueCache;
		}
		public static T GetUserInput<T>()
		{
			throw new NotImplementedException();
		}

		private static void FocusingOnCurrentEntity()
		{
			Resources.ActiveEntities.SetFocusStatus(Resources.ActiveEntities.Pointer, Focus.Focusing);
		}

		private static bool ExecuteConsoleKey()
		{
			if (isExit)
			{
				isExit = false;
				return false;
			}

			Resources.Output.SetCursorPosition(Resources.Output.BufferWidth - 1, 0);
			Resources.Output.ForegroundColor = Resources.DefaultSystemConsoleBackgroundColor;
			Resources.Output.BackgroundColor = Resources.DefaultSystemConsoleBackgroundColor;

			ConsoleKeyInfo key = Resources.Output.ReadKey();

			Resources.Output.ForegroundColor = Resources.DefaultSystemConsoleForegroundColor;
			Resources.Output.SetCursorPosition(
				Resources.SystemConsoleCursorPosition.X,
				Resources.SystemConsoleCursorPosition.Y
			);

			var entity = Resources.ActiveEntities.Get(Focus.Focusing, Focus.Focused);
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
						FocusingOnCurrentEntity();
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

		private static class Renderer
		{
			public static void Render()
			{
				var e = Resources.ActiveEntities;
				for (int k = 0; k < e.Count; k++)
				{
					var ps = e[k].GetRenderBuffer();
					for (int j = 0; j < ps.GetLength(1); j++)
					{
						for (int i = 0; i < ps.GetLength(0); i++)
						{
							Resources.Output.SetCursorPosition(i + e[k].Anchor.X, j + e[k].Anchor.Y);
							Resources.Output.ForegroundColor = ps[i, j].ForegroundColor;
							Resources.Output.BackgroundColor = ps[i, j].BackgroundColor;
							Resources.Output.Write(ps[i, j].Character);
						}
					}
				}
			}
		}

		internal static class Parser
		{
			public static List<object> Parse()
			{
				if (tryLiteralString(out List<object> result)) return result;
				else if (tryShellCommand(out result)) return result;
				else if (tryCSharpScript(out result)) return result;
				else throw new InvalidOperationException("Please Check Your Command");

				throw new NotImplementedException();
			}

			private static bool tryCSharpScript(out List<object> result)
			{
				try
				{

				}
				catch
				{
					result = null;
					return false;
				}

				throw new NotImplementedException();
			}

			private static bool tryLiteralString(out List<object> result)
			{
				try
				{

				}
				catch
				{
					result = null;
					return false;
				}

				throw new NotImplementedException();
			}
			private static bool tryShellCommand(out List<object> result)
			{
				try
				{

				}
				catch
				{
					result = null;
					return false;
				}

				throw new NotImplementedException();
			}
		}
	}

	public interface IOutput
	{
		ConsoleColor ForegroundColor { get; set; }
		ConsoleColor BackgroundColor { get; set; }
		Encoding OutputEncoding { get; set; }
		int CursorTop { get; set; }
		int CursorLeft { get; set; }
		bool CursorVisible { get; set; }
		int BufferWidth { get; set; }
		void SetCursorPosition(int left, int top);
		void Write(char input);
		void WriteLine();
		void Clear();
		ConsoleKeyInfo ReadKey();
	}

	public class ConsoleOutput : IOutput
	{
		public ConsoleColor ForegroundColor
		{
			get => SConsole.ForegroundColor;
			set => SConsole.ForegroundColor = value;
		}
		public ConsoleColor BackgroundColor
		{
			get => SConsole.BackgroundColor;
			set => SConsole.BackgroundColor = value;
		}
		public int CursorTop
		{
			get => SConsole.CursorTop;
			set => SConsole.CursorTop = value;
		}
		public int CursorLeft
		{
			get => SConsole.CursorLeft;
			set => SConsole.CursorLeft = value;
		}
		public Encoding OutputEncoding
		{
			get => SConsole.OutputEncoding;
			set => SConsole.OutputEncoding = value;
		}
		public int BufferWidth
		{
			get => SConsole.BufferWidth;
			set => SConsole.BufferWidth = value;
		}
		public bool CursorVisible
		{
			get => SConsole.CursorVisible;
			set => SConsole.CursorVisible = value;
		}
		public void SetCursorPosition(int left, int top) => SConsole.SetCursorPosition(left, top);
		public void Write(char input) => SConsole.Write(input);
		public void WriteLine() => SConsole.WriteLine();
		public void Clear() => SConsole.Clear();
		public ConsoleKeyInfo ReadKey() => SConsole.ReadKey();
	}
}