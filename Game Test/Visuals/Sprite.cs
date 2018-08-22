using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Graphics;

namespace Game_Test.Visuals
{
    class Sprite
    {
        public Surface Source { get; private set; }
        public Rectangle Region { get; private set; }

        public Sprite(Surface img) : this(img, new Rectangle(new Point(), img.Size))
        {
            
        }

        public Sprite(Surface source, Rectangle region)
        {
            Source = source;
            Region = region;
            //Source = ((Bitmap)image).Clone(region, image.PixelFormat);
            //Source = image;
            //Region = region;
        }

        public void DrawOnto(Surface g, Rectangle destination)
        {
            //g.FillRectangle(new SolidBrush(Color.White), destination);
            //g.DrawImage(Image, new Point[] { destination.Location, new Point(destination.Right, destination.Top),  /*new Point(destination.Right, destination.Bottom), */new Point(destination.Left, destination.Bottom)});
            //destination.Width++;
            //destination.Height++;
            //g.DrawImage(Source, destination, Region, GraphicsUnit.Pixel);
            //g.Blit(Sub.CreateScaledSurface(destination.Width / Region.Width), destination);
            g.Blit(Source, destination, Region);
            //g.DrawImage(Image, destination);
        }
    }
}
