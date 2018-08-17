using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Game_Test.Scenes;

namespace Game_Test
{
    class Game
    {
        public readonly Size ScreenSize = new Size(1280, 720);

        public Scene CurrentScene;
        private GameScreen _screen;

        private Stopwatch _sw;

        private bool _running = true;

        public bool Drawing { get; internal set; } = false;

        private Thread _gameLoopThread;

        public bool Started { get; internal set; } = false;

        public Game(Scene scene)
        {
            CurrentScene = scene;

            _screen = new GameScreen(this);


            _screen.FormClosing += (o, e) =>
            {
                if (_running) e.Cancel = true;
                _running = false;
            };

            _screen.FormClosed += (o, e) =>
            {
                Application.Exit();
            };

            StartGameLoopThread();

            Application.Run();
        }

        private void StartGameLoopThread()
        {
            _gameLoopThread = new Thread(new ThreadStart(StartGameLoop));
            _gameLoopThread.Start();
        }

        private void StartGameLoop()
        {
            while (!Started) { }

            //double limit = 1000 / TargetFps;

            _sw = new Stopwatch();

            _sw.Start();

            while (true)
            {

                float deltaTime = (float) (_sw.Elapsed.TotalMilliseconds / 1000d);
                _sw.Restart();

                CurrentScene.Tick(deltaTime);
                _screen.CrossThreadRefresh();

                if (!_running)
                {
                    _screen.CrossThreadClose();
                    break;
                }
            }
        }

        /*private void TickEventHandler(object sender, TickEventArgs e)
        {
            if (!finished) return;
            finished = false;
            float deltaTime = _sw.ElapsedMilliseconds / 1000f;
            _sw.Restart();
            CurrentScene.Tick(deltaTime);

            _screen.Fill(Color.Black);

            CurrentScene.Draw(_screen);
            
            _screen.Update();
            Video.WindowCaption = Events.Fps + " fps";
            finished = true;
        }

        private void QuitEventHandler(object sender, QuitEventArgs e) => Events.QuitApplication();*/
    }
}
