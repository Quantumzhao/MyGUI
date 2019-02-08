#define Test

using MyGUI;
using Console = System.Console;
using MyGUIConsole = MyGUI.Session.Console;

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
			MyGUIConsole.Prompt(new CheckBox("Test"));
			MyGUIConsole.Execute();
#elif true
			MyGUI.Session.Resources.ConsoleCommand = args;
			MyGUIConsole.Execute("Command1 -o1 -o2 arg1 arg2");
#endif
			Console.ReadLine();
		}
	}
}
