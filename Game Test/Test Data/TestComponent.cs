using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;

namespace Game_Test.Test_Data
{
    class TestComponent : IComponent
    {
        public TestComponent() => ComponentName = "test";
    }
}
