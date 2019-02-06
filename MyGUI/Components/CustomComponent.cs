using System;
using System.Collections.Generic;
using System.Text;

namespace MyGUI
{
	class CustomComponent
	{
		public Property Properties = new Property();
		public Method Methods = new Method();

		public class Property
		{
			private Dictionary<string, object> properties = new Dictionary<string, object>();

			public object this[string name]
			{
				get
				{
					return properties[name];
				}
				set
				{

				}
			}
		}

		public class Method
		{
			private Dictionary<string, object> methods = new Dictionary<string, object>();

			public object this[string name]
			{
				get
				{
					return methods[name];
				}
				set
				{

				}
			}
		}
	}
}
