using System.Drawing;
using System.Linq;

using Game_Test.Components;
using Game_Test.ComponentSystems;
using Game_Test.Visuals;

namespace Game_Test.Scenes
{
    class Scene
    {
        public readonly EntityMap Entities;
        public readonly SystemMap Systems;
        public readonly SpriteSet Sprites;

        public Viewport CurrentViewport { get; set; }

        public float DeltaTime { get; private set; } = 0;

        public Scene()
        {
            Entities = new EntityMap();
            Systems = new SystemMap(this);
            Sprites = new SpriteSet();

            Entities.Changed += new EntityChangedEventHandler(NotifyEntityChange);
            CurrentViewport = new Viewport();
        }

        public void Draw(Graphics g)
        {
            Systems.InvokeRender(g);
        }

        protected virtual void Tick()
        {

        }

        public void Tick(float deltaTime)
        {
            DeltaTime = deltaTime;
            Entities.Flush();

            Systems.InvokeTick();

            Tick();
        }

        private void NotifyEntityChange(EntityChangedEventArgs e)
        {
            foreach (IComponent component in e.Entity.Components)
            {
                foreach (ComponentSystem system in Systems)
                {
                    if (system.Watching.Contains(component.ComponentName))
                    {
                        if (e.WasRemoved) system.RemoveWatchedComponent(component);
                        else system.AddWatchedComponent(component);
                    }
                }
            }
        }
    }
}
