
using System.Drawing;
using Game_Test.Scenes;
using Game_Test.Visuals;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace Game_Test.Transition
{
    class NewGameScreen
    {
        public readonly ScreenRenderer Renderer;
        public Scene ActiveScene { get; private set; }

        public NewGameScreen(Scene scene)
        {
            this.ActiveScene = scene;
            Renderer = new ScreenRenderer();
            Events.Tick += TickEventHandler;
            Events.Quit += QuitEventHandler;
            Events.VideoResize += VideoResizeEventHandler;
            Events.TargetFps = 60;

            Events.Run();
        }

        private void TickEventHandler(object sender, TickEventArgs e)
        {
            ActiveScene.InvokeTick();

            Renderer.Render(ActiveScene);
        }

        private void QuitEventHandler(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        private void VideoResizeEventHandler(object sender, VideoResizeEventArgs e)
        {
            Renderer.SetScreenSize(new Size(e.Width, e.Height));
        }
    }
}
