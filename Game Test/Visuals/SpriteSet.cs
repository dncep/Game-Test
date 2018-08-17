using System.Collections.Generic;
using System.Drawing;

namespace Game_Test.Visuals
{
    class SpriteSet
    {
        private readonly List<SpriteSheet> _sheets = new List<SpriteSheet>();

        public Sprite this[string name] {
            get
            {
                foreach(SpriteSheet sheet in _sheets)
                {
                    Sprite contained = sheet[name];
                    if (!(contained is null)) return contained;
                }
                return null;
            }
        }

        public SpriteSet()
        {
        }

        public void AddSpriteSheet(SpriteSheet sheet) => _sheets.Add(sheet);
    }
}
