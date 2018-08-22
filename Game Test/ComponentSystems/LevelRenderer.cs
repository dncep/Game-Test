using System;
using System.Drawing;

using Game_Test.Components;
using Game_Test.Scenes;
using Game_Test.Visuals;

namespace Game_Test.ComponentSystems
{
    class LevelRenderer : ComponentSystem
    {
        public LevelRenderer()
        {
            SystemName = "level_renderer";
            Watching = new string[] { "level_tile" };
            RenderProcessing = true;
        }

        public override void Render(ScreenRenderer r)
        {
            r.BeginRenderingSection(true);

            Viewport view = Owner.CurrentViewport;
            
            foreach (LevelTile tile in WatchedComponents)
            {
                Renderable renderable = tile.Owner.Components["renderable"] as Renderable;
                renderable.Render(r, view, Owner.Sprites);
            }
            
            r.EndRenderingSection();
        }
    }
}
