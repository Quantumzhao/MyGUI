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
		public bool IsConventionalCliInterface { get; set; } = false;
	}

	public class Console
	{
		public static void Load(Component component)
		{

		}
		public static void Activate(string entityName)
		{

		}

		public static string Prompt(Component component)
		{
			Resources.ActiveEntities.Add(component);

			throw new NotImplementedException();
		}
		public static string Prompt()
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
