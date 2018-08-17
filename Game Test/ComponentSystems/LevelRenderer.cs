using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Game_Test.Components;
using Game_Test.Scenes;

namespace Game_Test.ComponentSystems
{
    class LevelRenderer : Renderer
    {
        public LevelRenderer()
        {
            SystemName = "level_renderer";
            Watching = new string[] { "level_tile" };
            RenderProcessing = true;
        }

        public override void Render(Graphics g)
        {
            Viewport view = Owner.CurrentViewport;

            GraphicsState old = g.Save();

            g.ScaleTransform(view.Scale, view.Scale);
            g.TranslateTransform(-view.X, -view.Y);
            g.TranslateTransform(g.VisibleClipBounds.Width / 2f, g.VisibleClipBounds.Height / 2);

            foreach (LevelTile tile in WatchedComponents)
            {
                Physical physical = tile.Owner.Components["physical"] as Physical;
                float x = (physical.X - 8);
                float y = (physical.Y - 8);
                float size = 16;

                Rectangle drawingRect = new Rectangle((int)Math.Round(x), (int)Math.Round(y), (int)Math.Ceiling(size), (int)Math.Ceiling(size));

                Owner.Sprites[tile.Texture].DrawOnto(g, drawingRect);
            }

            g.Restore(old);
        }
    }
}
