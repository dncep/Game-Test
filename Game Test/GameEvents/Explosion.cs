using Game_Test.Components;

namespace Game_Test.GameEvents
{
    class Explosion : Event
    {
        public Explosion(Component sender) : base("explosion", sender)
        {
        }
    }
}
