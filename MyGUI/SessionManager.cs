using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MyGUI.Utilities;

namespace MyGUI.Session
{
	public static class Resources
	{
		internal static List<IEntity> ActiveEntities { get; set; } = new List<IEntity>();

		public static string ReturnValueCache { get; internal set; } = null;
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
					/*static BorderStyle()
					{
						BoarderStyle.Add(BoarderCharPos.UpperLeft, '╔');
						BoarderStyle.Add(BoarderCharPos.LowerLeft, '╚');
						BoarderStyle.Add(BoarderCharPos.UpperRight, '╗');
					}*/

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
			public static readonly ConsoleColor HighlightForegroundColor = ConsoleColor.White;
			public static readonly ConsoleColor HighlightBackgroundColor = ConsoleColor.Blue;
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
		public static string Prompt()
		{
			throw new NotImplementedException();
		}

		public static string GetUserInput()
		{
			throw new NotImplementedException();
		}
		public T GetUserInput<T>()
		{
			throw new NotImplementedException();
		}

		private static void redirectInput(PrimitiveComponent component)
		{

		}
		private static void redirectInput()
		{

		}

		private static void Render(PrimitiveComponent component)
		{

		}
	}
}
