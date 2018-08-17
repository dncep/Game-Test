using System;

namespace Game_Test.Components
{
    class Health : IComponent
    {
        public Health() => ComponentName = "health";

        public int MaxHealth
        {
            get
            {
                return MaxHealth;
            }
            set
            {
                MaxHealth = Math.Max(1, value);
                CurrentHealth = Math.Min(MaxHealth, CurrentHealth);
            }
        }

        public int CurrentHealth {
            get {
                return CurrentHealth;
            }
            set
            {
                CurrentHealth = Math.Max(0, value);
            }
        }
    }
}
