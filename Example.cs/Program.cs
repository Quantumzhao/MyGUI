#define Test

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
#if Test
			/*MyGUIConsole.Prompt(
				new ListBox(
					new ListItem("Name1"),
					new ListItem("Name2", "v2")
				)
			);*/
			MyGUIConsole.Execute();
#elif true
			MyGUI.Session.Resources.ConsoleCommand = args;
			MyGUIConsole.Execute("Command1 -o1 -o2 arg1 arg2");
#endif
			Console.ReadLine();
		}
	}
}
