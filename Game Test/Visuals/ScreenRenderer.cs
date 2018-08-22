using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Scenes;
using Game_Test.Util;
using SdlDotNet.Graphics;

namespace Game_Test.Visuals
{
    class ScreenRenderer
    {
        public Size ScreenSize { get; private set; } = new Size(1280, 720);
        public Size ScreenResolution = new Size(320, 180);
        private readonly int LockedResolutionWidth = 320;

        public Surface View { get; private set; }
        public Surface Screen { get; private set; }
        private Surface viewToScreen;

        private bool scaleToViewport = false;
        private Scene scene;

        public ScreenRenderer()
        {
            SetScreenSize(ScreenSize);
        }

        internal void Render(Scene activeScene)
        {
            scene = activeScene;
            Screen.Fill(Color.Black);

            activeScene.Draw(this);

            Screen.Update();
            scene = null;
            
        }

        public void BeginRenderingSection(bool scaleToViewport)
        {
            this.scaleToViewport = scaleToViewport;
            View.Fill(View.TransparentColor);
        }

        public void EndRenderingSection()
        {
            double scale = (double) ScreenSize.Width / ScreenResolution.Width;
            if (scaleToViewport) scale *= scene.CurrentViewport.Scale;

            Point destination = new Point(0, 0);
            if(scaleToViewport)
            {
                destination.X -= (int)Math.Round((ScreenResolution.Width * scale - ScreenSize.Width) / 2);
                destination.Y -= (int)Math.Round((ScreenResolution.Height * scale - ScreenSize.Height) / 2);
                destination.X -= (int)(scale * GeneralUtil.FloorMod(scene.CurrentViewport.X, 1));
                destination.Y += (int)(scale * GeneralUtil.FloorMod(scene.CurrentViewport.Y, 1));
            }

            viewToScreen = View.CreateScaledSurface(scale, false);
            viewToScreen.Transparent = true;
            Screen.AlphaBlending = true;
            Screen.Blit(viewToScreen, destination);
            viewToScreen.Dispose();
            viewToScreen = null;
        }

        internal void SetScreenSize(Size newSize)
        {
            ScreenSize = newSize;
            if (Screen != null) Screen.Dispose();
            Screen = Video.SetVideoMode(ScreenSize.Width, ScreenSize.Height, true, false, false, true);

            ScreenResolution.Width = LockedResolutionWidth;
            ScreenResolution.Height = (int) (newSize.Height / ((float) newSize.Width / LockedResolutionWidth));

            if (View != null) View.Dispose();
            View = new Surface(new Size(ScreenResolution.Width + 1, ScreenResolution.Height + 1));
            View.Transparent = true;
            View.TransparentColor = Color.Empty;
        }
    }
}
