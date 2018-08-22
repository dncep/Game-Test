using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.Entities;
using Game_Test.Util;

namespace Game_Test.Test_Data
{
    class Slope : Entity
    {
        public Slope(float x, float y, bool left)
        {
            Components.Add(new Spatial(16*x, 16*y));
            Components.Add(new Renderable(left ? "brick_slope_left" : "brick_slope_right", new System.Drawing.Rectangle(-8, -8, 16, 16)));
            Components.Add(new RigidBody(new Polygon(new Vector2D(8, -8), new Vector2D(-8, -8), new Vector2D(left ? 8 : -8, 8)), 4f, true));
            Components.Add(new LevelTile());
        }
    }
}
