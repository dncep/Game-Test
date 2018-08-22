using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game_Test.Scenes;

namespace Game_Test
{
    class GameScreen : Form
    {
        /*private Game _game;

        public GameScreen(Game game)
        {
            this._game = game;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, false);

            this.Size = _game.ScreenSize;

            Show();

            game.Started = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.Black), g.VisibleClipBounds);
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            _game.CurrentScene.Draw(g);

            _game.Drawing = false;
        }

        internal void CrossThreadRefresh() => Invoke((MethodInvoker)delegate { Refresh(); });
        internal void CrossThreadClose() => Invoke((MethodInvoker)delegate { Close(); });*/
    }
}
