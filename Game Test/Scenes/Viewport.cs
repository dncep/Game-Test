using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Test.Scenes
{
    class Viewport
    {
        public float X { get; set; }
        public float Y { get; set; }
        private float _scale = 1;
        public float Scale {
            get => _scale;
            set
            {
                if(value > 0) _scale = value;
            }
        }
    }
}
