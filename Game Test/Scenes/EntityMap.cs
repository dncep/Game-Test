﻿using Game_Test.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game_Test.Scenes.EntityChangedEventArgs;

namespace Game_Test.Scenes
{
    class EntityMap : IEnumerable<Entity>
    {
        private readonly Dictionary<int, Entity> _dict = new Dictionary<int, Entity>();
        private readonly Dictionary<int, Entity> _scheduledAdd = new Dictionary<int, Entity>();
        private readonly List<int> _scheduledRemove = new List<int>();

        public void Flush()
        {
            foreach (KeyValuePair<int, Entity> item in _scheduledAdd)
            {
                _dict.Add(item.Key, item.Value);
                Changed.Invoke(new EntityChangedEventArgs(item.Value, false));
            }
            _scheduledAdd.Clear();
            foreach (int id in _scheduledRemove)
            {
                Changed.Invoke(new EntityChangedEventArgs(_dict[id], true));
                _dict.Remove(id);
            }
            _scheduledRemove.Clear();
        }

        public event EntityChangedEventHandler Changed;


        public Entity this[int key] { get => _dict[key]; }

        public int Count => _dict.Count;

        public void Add(Entity entity)
        {
            _scheduledAdd[entity.Id] = entity;
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool ContainsId(int id)
        {
            return _dict.ContainsKey(id);
        }

        public bool Contains(Entity entity)
        {
            return ContainsId(entity.Id);
        }

        public bool Remove(int id)
        {
            if (ContainsId(id))
            {
                _scheduledRemove.Add(id);
                return true;
            }
            return false;
        }

        public bool Remove(Entity entity)
        {
            if (Contains(entity))
            {
                _scheduledRemove.Add(entity.Id);
                return true;
            }
            return false;
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _dict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
    }

    delegate void EntityChangedEventHandler(EntityChangedEventArgs e);

    class EntityChangedEventArgs : EventArgs
    {
        public readonly Entity Entity;
        public bool WasRemoved { get; private set; }

        public EntityChangedEventArgs(Entity entity, bool removed)
        {
            this.Entity = entity;
            this.WasRemoved = removed;
        }
    }
}
