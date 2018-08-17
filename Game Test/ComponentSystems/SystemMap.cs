using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Game_Test.Components;
using Game_Test.Entities;
using Game_Test.Scenes;

namespace Game_Test.ComponentSystems
{
    class SystemMap : IEnumerable<ComponentSystem>
    {
        private readonly Scene Owner;
        private readonly Dictionary<string, ComponentSystem> _dict = new Dictionary<string, ComponentSystem>();

        private readonly List<ComponentSystem> _inputSystems = new List<ComponentSystem>();
        private readonly List<ComponentSystem> _tickSystems = new List<ComponentSystem>();
        private readonly List<ComponentSystem> _renderSystems = new List<ComponentSystem>();

        public SystemMap(Scene owner)
        {
            Owner = owner;
        }

        public ComponentSystem this[string name] => _dict[name];

        public int Count => _dict.Count;
        public void Add(ComponentSystem system) {
            _dict.Add(system.SystemName, system);
            system.Owner = Owner;

            if (system.InputProcessing) _inputSystems.Add(system);
            if (system.TickProcessing) _tickSystems.Add(system);
            if (system.RenderProcessing) _renderSystems.Add(system);
            
            foreach (Entity entity in Owner.Entities)
            {
                foreach(IComponent component in entity.Components)
                {
                    if(system.Watching.Contains(component.ComponentName))
                    {
                        system.AddWatchedComponent(component);
                    }
}
            }
        }

        public void Clear() => _dict.Clear();
        public bool Contains(string name) => _dict.ContainsKey(name);
        public bool Remove(string name) => _dict.Remove(name);

        internal void InvokeTick() => _tickSystems.ForEach((s) => s.Tick());
        internal void InvokeRender(Graphics g) => _renderSystems.ForEach((s) => s.Render(g));

        public IEnumerator<ComponentSystem> GetEnumerator() => _dict.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
    }
}
