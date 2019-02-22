using System;
using MyGUI.Session;
using MyGUI.Utilities;
using System.Collections.Generic;

namespace MyGUI
{
	public class LogicGroup : Container<PrimitiveComponent>
	{
		public LogicGroup(params PrimitiveComponent[] components)
		{
			foreach (var item in components)
			{
				item.SetParent(this);
			}
			Collection.AddRange(components);

			Resources.ActiveEntities.Add(this);

			if (components[0] is RadioButton)
			{
				OnElementChange = collection =>
				{
					collection.ForEach(c => (c as RadioButton).Value = false);
					return true;
				};
			}
		}

		public event Func<AbstractCollection<PrimitiveComponent>, bool> OnElementChange;

		public override Pixel[,] GetRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			throw new NotImplementedException();
		}

		public override void UpdateRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public void UpdateComponentList<T>() where T : PrimitiveComponent
		{

		}
	}
}
