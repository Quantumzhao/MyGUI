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
					this.value = value;
					OnValueChanged?.Invoke(value);
				}
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
			if (!base.ParseAndExecute(key))
			{
				switch (key.Key)
				{
					case ConsoleKey.Enter:
						Value = true;
						break;

					default:
						return false;
				}
			}
			return true;
		}
	}
}
