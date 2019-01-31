using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;

namespace MyGUI
{
	class ComboBox : Container
	{
		public bool IsAcceptTextInput { get; set; }
		public List<ListItem> List { get; set; } = new List<ListItem>();
	}

	class ListItem
	{
		public string Name { get; set; }

		public void OnSelect(object sender, EventArgs args)
		{
			
		}
	}
}
