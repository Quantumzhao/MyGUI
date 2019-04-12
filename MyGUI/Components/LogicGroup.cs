using MyGUI.Session;
using MyGUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGUI
{
    public class LogicGroup<V> : Group<PrimitiveComponentWithValue<V>>, IValue<IEnumerable<V>>
    {
        public IEnumerable<V> Value
        {
            get => Collection.Select(c => c.Value);
        }

        public LogicGroup(params PrimitiveComponentWithValue<V>[] components)
        {
            foreach (var item in components)
            {
                item.SetParent(this);
                //item.OnValueChanged += OnElementChanged;
            }
            Collection.AddRange(components);

            Resources.ActiveEntities.Add(this);
        }

        public event Action<IEnumerable<V>> OnValueChanged;

        public event Action<PrimitiveComponentWithValue<V>, V> OnElementChanged;

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
