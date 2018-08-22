using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.Util;

namespace Game_Test.Entities
{
    class TileEntity : Entity
    {
        public TileEntity(string texture, int tileX, int tileY)
        {
            Components.Add(new Spatial(16*tileX, 16*tileY));
            Components.Add(new RigidBody(new Rectangle(-8, -8, 16, 16), 1f, true));
            Components.Add(new Renderable(texture, new System.Drawing.Rectangle(-8, -8, 16, 16)));
            Components.Add(new LevelTile());
        }

        public override string ToString() => "TileEntity{Texture=" + (Components["renderable"] as Renderable).Texture + ",Position=" + (Components["spatial"] as Spatial).Position + "}";
    }
}
