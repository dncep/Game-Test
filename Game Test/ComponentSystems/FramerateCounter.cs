using System;
using System.Drawing;

namespace Game_Test.ComponentSystems
{
    class FramerateCounter : ComponentSystem
    {
        private float _elapsed = 0;
        private int frames = 0;
        public int Framerate { get; private set; }

        public FramerateCounter()
        {
            SystemName = "fpscounter";
            TickProcessing = true;
            RenderProcessing = true;
        }

        public override void Tick()
        {
            frames++;
            _elapsed += Owner.DeltaTime;
            //Console.WriteLine("deltatime:" + Owner.DeltaTime);
            if (_elapsed >= 1)
            {
                _elapsed--;
                Framerate = frames;
                frames = 0;
                Console.WriteLine($"{Framerate} fps");
            }
        }
        public override void Render(Graphics g)
        {

            g.DrawString($"{Framerate} fps", new Font("Consolas", 12), new SolidBrush(Color.Yellow), 0, 0);
        }
    }
}
