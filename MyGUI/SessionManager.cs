using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MyGUI.Utilities;
using CustomizedFunction;

namespace MyGUI.Session
{
	public static class Resources
	{
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
					static SeparatorStyle()
					{

					}

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

	public class Console
	{
		// Uncapsulated version of "Prompt"
		public static void Load(PrimitiveComponent component)
		{

		}
		public static void Activate(string entityName)
		{

		}

		public static string Prompt(IEntity entity)
		{
			Resources.ActiveEntities.Add(entity);

			if (Settings.IsConventionalCliInterface)
			{

			}

			throw new NotImplementedException();
		}

		private static void Main()
		{
			if (!Settings.IsConventionalCliInterface)
			{
				while (true)
				{
					ExecuteConsoleKey();
				}
			}
			else
			{
				ExecuteCommand(System.Console.ReadLine());
			}
		}

		private static void SelectPrevEntity()
		{

		}

		private static void SelectNextEntity()
		{

		}

		public static string GetUserInput()
		{
			Main();

			return Resources.ReturnValueCache;
		}
		public T GetUserInput<T>()
		{
			throw new NotImplementedException();
		}

		private static void FocusingOnCurrentEntity()
		{

		}

		private static void ExecuteCommand(string command)
		{

		}

		private static void ExecuteConsoleKey()
		{
			ConsoleKeyInfo key = System.Console.ReadKey();
			var entity = Resources.ActiveEntities.GetFocusing();
			if (entity == null)
			{
				Resources.ActiveEntities.SetFocusing(0);
			}
			if (entity.ParseAndExecute(key))
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
						return;

					default:
						break;
				}

			}
		}

		private static void Render(PrimitiveComponent component)
		{

		}

		internal static class Parser
		{
			
		}
	}
}
