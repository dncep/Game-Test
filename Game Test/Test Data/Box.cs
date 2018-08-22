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
    class Box : Entity
    {
        public Box(float x, float y)
        {
            Components.Add(new Spatial(x, y));
            Components.Add(new Renderable("brick_slope_upside_down", new System.Drawing.Rectangle(-8, -8, 16, 16)));
            Components.Add(new RigidBody(new Polygon(new Vector2D(-8, 8), new Vector2D(8, 8), new Vector2D(8, -8)), 4f, false));
            Components.Add(new LevelTile());
        }

        public override string ToString() => "Box{Texture=" + (Components["renderable"] as Renderable).Texture + ",Position=" + (Components["spatial"] as Spatial).Position + "}";
    }
}
