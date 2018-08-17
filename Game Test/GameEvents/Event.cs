using Game_Test.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Test.GameEvents
{
    class Event
    {
        public string EventType { get; }
        public Component Sender { get; }
        public Entity Receiver { get; }
    }
}
