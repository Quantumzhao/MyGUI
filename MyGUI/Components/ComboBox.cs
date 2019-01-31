using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;

namespace MyGUI
{
	class ComboBox : Container<ListItem>
	{
		public ComboBox(params ListItem[] item)
		{

		}

		public bool IsAcceptTextInput { get; set; }
	}

	class ListItem : Component
	{
		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public void OnSelect(object sender, EventArgs args)
		{
			
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}
	}
}
