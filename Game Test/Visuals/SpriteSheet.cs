using System;
using System.Collections.Generic;
using System.Drawing;
using SdlDotNet.Graphics;

namespace Game_Test.Visuals
{
    class SpriteSheet
    {
        private readonly Surface _image;
        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

        public Sprite this[string name]
        {
            get
            {
                return _sprites.ContainsKey(name) ? _sprites[name] : null;
            }
        }

        public SpriteSheet(Image image)
        {
            _image = new Surface(new Bitmap(image));
        }

        public void AddRegion(string name, Rectangle rectangle)
        {
            _sprites.Add(name, new Sprite(_image, rectangle));
        }

        public void AddRegion(string name, int x, int y, int w, int h)
        {
            AddRegion(name, new Rectangle(x, y, w, h));
        }
    }
}
