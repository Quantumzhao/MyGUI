using System;
using MyGUI;
using System.Threading;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Thread thread = new Thread(Left);
			thread.Start();
			Right();

			Console.ReadLine();
		}

		static void Right()
		{
			for (int i = 0; i < 20; i++)
			{
				Console.CursorLeft = 10;
				Console.Write("#");
				Console.CursorTop++;
			}
		}

		static void Left()
		{
			for (int i = 0; i < 20; i++)
			{
				Console.CursorLeft = 0;
				Console.Write("@");
				Console.CursorTop++;
			}
		}
	}
}
