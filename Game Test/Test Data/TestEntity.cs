using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.Entities;

namespace Game_Test.Test_Data
{
    class TestEntity : Entity
    {
        public TestEntity(int x, int y)
        {
            Components.Add(new Spatial(x, y));
            Components.Add(new Renderable("note", new Rectangle(-8, -8, 16, 16)));
            Components.Add(new TestComponent());
        }
    }
}
