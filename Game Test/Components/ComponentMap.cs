using System.Collections;
using System.Collections.Generic;

using Game_Test.Entities;

namespace Game_Test.Components
{
    class ComponentMap : IEnumerable<Component>
    {
        private readonly Entity _owner;
        private readonly Dictionary<string, Component> _dict = new Dictionary<string, Component>();

        public ComponentMap(Entity owner)
        {
            _owner = owner;
        }

        public Component this[string name] => _dict[name];

        public int Count => _dict.Count;
        public void Add(Component component)
        {
            _dict.Add(component.ComponentName, component);
            component.Owner = _owner;
        }
        public void Clear() => _dict.Clear();
        public bool Contains(string name) => _dict.ContainsKey(name);
        public bool Remove(string name) => _dict.Remove(name);

        public IEnumerator<Component> GetEnumerator() => _dict.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

    }
}
