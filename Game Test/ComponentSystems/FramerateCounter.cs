using System.Drawing;

using Game_Test.Visuals;

using SdlDotNet.Graphics;

using Font = SdlDotNet.Graphics.Font;

namespace Game_Test.ComponentSystems
{
    class FramerateCounter : ComponentSystem
    {
        private float _elapsed = 0;
        private int frames = 0;
        public int Framerate { get; private set; }

        private Font font;

        public FramerateCounter()
        {
            SystemName = "fpscounter";
            TickProcessing = true;
            RenderProcessing = true;

            font = new Font(Properties.Resources.consola, 11);
        }

        public override void Tick()
        {
            frames++;
            _elapsed += Owner.DeltaTime;
            if (_elapsed >= 1)
            {
                _elapsed--;
                Framerate = frames;
                frames = 0;
            }
        }
        public override void Render(ScreenRenderer r)
        {
            r.BeginRenderingSection(false);

            Surface textSurface = font.Render($"{Framerate} fps", Color.Yellow);
            r.View.Blit(textSurface, new Point(0, 0));
            textSurface.Dispose();
            //g.DrawString(, new Font("Consolas", 12), new SolidBrush(Color.Yellow), 0, 0);

            r.EndRenderingSection();
        }
    }
}
