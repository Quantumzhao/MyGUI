using System;
using MyGUI.Utilities;

namespace MyGUI
{
	public class RadioButton : Switch
	{
		public override event Action<bool> OnValueChanged;
		private bool value;
		public override bool Value
		{
			get => value;
			set
			{
				if (this.value != value)
				{
					OnValueChanged?.Invoke(value);
					this.value = value;
				}
			}
		}

		public RadioButton()
		{
			if (parent is LogicGroup<bool> p)
			{
				p.OnElementChanged += (PrimitiveComponentWithValue<bool> c, bool v) =>
				{
					if (v)
					{
						p.Collection.ForEach(e => e.Value = false);
						c.Value = true;
					}
				};
			}
		}

		private Pixel[,] renderBuffer;
		public override Pixel[,] GetRenderBuffer()
		{
			return renderBuffer;
		}
		private void initRenderBuffer()
		{
			renderBuffer = new Pixel[Width, Height];
			for (int j = 0; j < Height; j++)
			{
				for (int i = 0; i < Width; i++)
				{
					renderBuffer[i, j] = new Pixel();
				}
			}
			UpdateRenderBuffer();
		}
		public override void UpdateRenderBuffer()
		{
			throw new NotImplementedException();
		}

		public override bool ParseAndExecute(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.Enter:
					Value = true;
					break;

				default:
					return false;
			}
			return true;
		}
	}
}
