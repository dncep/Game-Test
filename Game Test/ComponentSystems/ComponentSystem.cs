﻿using System.Collections.Generic;

using Game_Test.Components;
using Game_Test.Scenes;
using Game_Test.Visuals;

namespace Game_Test.ComponentSystems
{
    abstract class ComponentSystem
    {
        private const byte _input = 0b001;
        private const byte _tick = 0b010;
        private const byte _render = 0b100;

        public Scene Owner { get; set; }
        public string[] Watching { get; protected set; } = new string[0];
        public string SystemName { get; protected set; }
        private byte _processing_loop = 0;

        public bool InputProcessing
        {
            get => (_processing_loop & _input) != 0;
            set => _processing_loop = (byte)(value ? (_processing_loop | _input) : (_processing_loop & ~_input));
        }
        public bool TickProcessing
        {
            get => (_processing_loop & _tick) != 0;
            set => _processing_loop = (byte)(value ? (_processing_loop | _tick) : (_processing_loop & ~_tick));
        }
        public bool RenderProcessing
        {
            get => (_processing_loop & _render) != 0;
            set => _processing_loop = (byte)(value ? (_processing_loop | _render) : (_processing_loop & ~_render));
        }

        protected List<Component> WatchedComponents = new List<Component>();

        public virtual void RemoveWatchedComponent(Component component)
        {
            WatchedComponents.Remove(component);
        }

        public virtual void AddWatchedComponent(Component component)
        {
            WatchedComponents.Add(component);
        }

        public virtual void Tick()
        {
        }

        public virtual void Render(ScreenRenderer g)
        {
        }
    }
}
