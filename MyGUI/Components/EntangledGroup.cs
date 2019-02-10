using System;
using System.Collections.Generic;
using System.Text;
using MyGUI.Utilities;

namespace MyGUI
{
	public class EntangledGroup<TValue> : Container<PrimitiveComponentWithValue<TValue>>, IValue<IEnumerable<TValue>>
	{
		public IEnumerable<object> Value { get; set; }
		IEnumerable<TValue> IValue<IEnumerable<TValue>>.Value { get; set; }

		public event Action<IEnumerable<object>> OnValueChanged;

		event Action<IEnumerable<TValue>> IValue<IEnumerable<TValue>>.OnValueChanged
		{
			add
			{
				throw new NotImplementedException();
			}

			remove
			{
				throw new NotImplementedException();
			}
		}

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
