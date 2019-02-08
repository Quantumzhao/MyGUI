using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using MyGUI.Utilities;
using SConsole = System.Console;
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

		internal static AbstractCollection<IEntity> ActiveEntities { get; set; } = new AbstractCollection<IEntity>();
		internal static int currentEntity;
		
		public static string ReturnValueCache { get; internal set; } = null;

		public static string[] ConsoleCommand { get; set; }
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

		public static bool IsConventionalCliInterface { get; set; } = false;

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
		}

		private static List<Pixel[,]> renderBuffer;
		private static List<MyPoint> updateChunk = new List<MyPoint>();

		// Uncapsulated version of "Prompt"
		public static void Load(PrimitiveComponent component)
		{

		}
		public static void Activate(string entityName)
		{

		}

		public static void Prompt(params IEntity[] entities)
		{
			MyPoint tempAnchor = new MyPoint(0, SConsole.CursorTop + 1);
			Resources.ActiveEntities.AddRange(
				entities.Select(e => {
					e.Anchor = tempAnchor;
					tempAnchor = new MyPoint(tempAnchor.X, tempAnchor.Y + e.Height);
					return e;
				})
			);

			Render();
		}
		public static void Prompt(MyPoint anchor, IEntity entity)
		{

		}

		public static void Execute(string command = null)
		{
			if (command == null)
			{
				Main();
			}
			else
			{
				ExecuteCommand(command);
			}
		}

		private static void Main()
		{
			while (ExecuteConsoleKey()) { }
		}

		private static void SelectPrevEntity()
		{
			Resources.ActiveEntities.Pointer--;
		}

		private static void SelectNextEntity()
		{
			Resources.ActiveEntities.Pointer++;
		}

		public static string GetUserInput()
		{
			Main();

			return Resources.ReturnValueCache;
		}
		public static T GetUserInput<T>()
		{
			throw new NotImplementedException();
		}

		private static void FocusingOnCurrentEntity()
		{
			Resources.ActiveEntities.SetFocusing(Resources.ActiveEntities.Pointer);
		}

		private static void ExecuteCommand(string command)
		{

		}

		private static bool ExecuteConsoleKey()
		{
			ConsoleKeyInfo key = SConsole.ReadKey();
			var entity = Resources.ActiveEntities.GetFocusing();
			if (entity == null)
			{
				switch (key.Key)
				{
					case ConsoleKey.UpArrow:
					case ConsoleKey.LeftArrow:
						SelectPrevEntity();
						break;

					case ConsoleKey.DownArrow:
					case ConsoleKey.RightArrow:
						SelectNextEntity();
						break;

					case ConsoleKey.Enter:
						FocusingOnCurrentEntity();
						break;

					case ConsoleKey.Escape:
						return false;

					default:
						break;
				}
			}
			else if (!entity.ParseAndExecute(key))
			{

			}
			else
			{

			}

			return true;
		}

		private static void Render(PrimitiveComponent component)
		{

		}
		private static void Render()
		{
			AbstractCollection<IEntity> entityList = Resources.ActiveEntities;
			for (int i = 0; i < entityList.Count; i++)
			{
				IEntity e = entityList[i];
				updateChunk.AddRange(
					e.UpdateChunks.Select(
						p => new MyPoint(
							p.X + e.Anchor.X, 
							p.Y + e.Anchor.Y
						)
					)
				);
			}

			Dictionary<MyPoint, Pixel> updatePixelBuffer = new Dictionary<MyPoint, Pixel>();
		}

		internal static class Parser
		{
			public static List<object> Parse()
			{
				throw new NotImplementedException();
			}
		}
	}
}
