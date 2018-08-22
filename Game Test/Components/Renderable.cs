using System;
using System.Drawing;

using Game_Test.Scenes;
using Game_Test.Visuals;
using SdlDotNet.Graphics;

namespace Game_Test.Components
{
    class Renderable : Component
    {
        public string Texture { get; set; }
        public Rectangle Bounds { get; set; }

        public Renderable() => ComponentName = "renderable";

        public Renderable(string texture) : this()
        {
            this.Texture = texture;
        }

        public Renderable(string texture, Rectangle bounds) : this()
        {
            this.Texture = texture;
            this.Bounds = bounds;
        }

        public void Render(ScreenRenderer r, Viewport view, SpriteSet sprites)
        {
            Spatial physical = Owner.Components["spatial"] as Spatial;
            float x = ((float) physical.X - 8);
            float y = ((float) -physical.Y - 8);
            int size = 16;

            x -= view.X;
            y += view.Y;

            x += r.ScreenResolution.Width / 2;
            y += r.ScreenResolution.Height / 2;

            Rectangle drawingRect = new Rectangle((int)Math.Floor(x), (int)Math.Floor(y), size, size);

            sprites[Texture].DrawOnto(r.View, drawingRect);
        }
    }
}
