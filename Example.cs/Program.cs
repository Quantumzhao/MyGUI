using System;
using MyGUI;
using MyGUIConsole = MyGUI.Session.Console;
using MyGUI.Utilities;
using System.Threading;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
#if true
			MyGUIConsole.Prompt(
				new ListBox(ListBox.DefaultHeight, ListBox.DefaultWidth,
					new ListBox.ListItem("Name1", "v1"),
					new ListBox.ListItem("Name2", "v2")
				)
			);
			string input = MyGUIConsole.GetUserInput();
#endif
			Console.ReadLine();
		}
	}
}
