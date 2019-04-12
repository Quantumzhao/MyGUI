using MyGUI;
using System;
using System.Collections.Generic;
using Console = System.Console;
using MyGUIConsole = MyGUI.Session.Console;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
#if false
			MyGUIConsole.Prompt(
				new ListBox(
					new ListItem("Name1"),
					new ListItem("Name2", "v2")
				)
			);
#elif false
			var tb = new TextBox(15, "tb", "");
			var btn = new Button(3, 30, "btn", "Press me",
				() =>
				{
					var cb = new CheckBox(tb.Value);
					MyGUIConsole.Prompt(cb);
				}
			);
			tb.OnValueChanged += s => btn.LabelComponent.Value = s;
			MyGUIConsole.Prompt(tb,btn);
			MyGUIConsole.Execute();
#elif true
			Console.WriteLine("The best language in the world:");
			MyGUIConsole.Prompt(
				new CheckBox("C#"),
				new CheckBox("PHP"),
				new CheckBox(1, 17, false, "Not Javascript")
			);

			MyGUIConsole.Prompt(new Button(3, 8, "Button", "Submit", () => 
			{
				if (!Resource.GetComponent<CheckBox>("C#").Value)
				{
					Resource.GetComponent<CheckBox>("C#").Value = true;
					Console.WriteLine("NO, C# is the best. Let's have a fight");
				}
				else
				{
					Console.WriteLine("Yes I agree");
				}
			}));

			MyGUIConsole.Execute();
#elif true
			MyGUI.Session.Resources.ConsoleCommand = args;
			MyGUIConsole.Execute("Command1 -o1 -o2 arg1 arg2");
#endif
			Console.ReadLine();
		}
	}
}
