using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;

namespace MyGUI.Session
{
	class Resources
	{
		public static List<IEntity> ActiveEntities { get; set; } = new List<IEntity>();
	}

	class Settings
	{
		public static bool IsConventionalCliInterface { get; set; } = false;
	}

	public class Console
	{
		// Uncapsulated version of "Prompt"
		public static void Load(Component component)
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

		private static void redirectInput(Component component)
		{

		}
		private static void redirectInput()
		{

		}

		private static void Render(Component component)
		{

		}

		private struct Chunk
		{
			public Coordinates Anchor { get; set; }
			public int Height { get; set; }
			public int Width { get; set; }
		}
	}
}
