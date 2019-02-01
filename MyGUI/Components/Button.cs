using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;

namespace MyGUI
{
	class Button : Component
	{
		public event Action OnClick;

		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}
	}
}
