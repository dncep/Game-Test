using Game_Test.Entities;

namespace Game_Test.Components
{
    abstract class IComponent
    {
        public Entity Owner { get; set; }
        public string ComponentName { get; protected set; }
    }
}
