using System;
using System.Collections.Generic;
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
            Components.Add(new Physical(x, y));
            Components.Add(new Renderable("note", 10 + (Id % 20)));
            Components.Add(new TestComponent());
        }
    }
}
