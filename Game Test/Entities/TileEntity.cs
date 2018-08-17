using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;

namespace Game_Test.Entities
{
    class TileEntity : Entity
    {
        public TileEntity(string texture, int tileX, int tileY)
        {
            Components.Add(new Physical(16*tileX, 16*tileY));
            Components.Add(new LevelTile(texture));
        }
    }
}
