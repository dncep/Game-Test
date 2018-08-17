using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.ComponentSystems;

namespace Game_Test.Test_Data
{
    class TestSystem : ComponentSystem
    {
        public TestSystem()
        {
            SystemName = "test";
            Watching = new string[] { "test" };
            TickProcessing = true;
        }

        public override void Tick()
        {
            foreach(TestComponent component in WatchedComponents)
            {
                if(component.Owner.Components["physical"] is Physical phys)
                {
                    phys.X += 5 * (component.Owner.Id % 20) * Owner.DeltaTime;
                }
            }
        }
    }
}
