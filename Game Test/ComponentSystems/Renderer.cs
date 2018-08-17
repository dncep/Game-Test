using System.Drawing;

using Game_Test.Components;
using Game_Test.Scenes;

namespace Game_Test.ComponentSystems
{
    class Renderer : ComponentSystem
    {
        public Renderer()
        {
            Watching = new string[] { "renderable" };
            SystemName = "renderer";
            RenderProcessing = true;
        }

        public bool Ready { get; private set; } = true;

        public override void Render(Graphics g)
        {
            if (!Ready) return;
            Ready = false;

            Viewport view = Owner.CurrentViewport;
            
            foreach(Renderable renderable in WatchedComponents)
            {
                renderable.Render(g, view, Owner.Sprites);
            }

            Ready = true;
        }
    }
}
