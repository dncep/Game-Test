using System;
using System.Drawing;

using Game_Test.Scenes;
using Game_Test.Visuals;

namespace Game_Test.Components
{
    class Renderable : IComponent
    {
        private float _scale = 1;
        public string Texture { get; set; }

        public float Scale
        {
            get => _scale;
            set
            {
                if (value != 0) _scale = value;
            }
        }

        public Renderable() => ComponentName = "renderable";

        public Renderable(string texture) : this()
        {
            this.Texture = texture;
        }

        public Renderable(string texture, int scale) : this()
        {
            this.Texture = texture;
            this.Scale = scale;
        }

        public void Render(Graphics g, Viewport view, SpriteSet sprites)
        {

            Physical physical = Owner.Components["physical"] as Physical;
            float x = (physical.X - 8);
            float y = (physical.Y - 8);
            float size = 16;

            Rectangle drawingRect = new Rectangle((int)Math.Round(x), (int)Math.Round(y), (int)Math.Ceiling(size), (int)Math.Ceiling(size));
            sprites[Texture].DrawOnto(g, drawingRect);
        }
    }
}
