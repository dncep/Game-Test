using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Test.Util
{
    public class Rectangle : IPolygon, IEquatable<Rectangle>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public double Left => X;
        public double Right => X + Width;
        public double Top => Y + Height;
        public double Bottom => Y;

        public Rectangle() : this(0, 0, 0, 0)
        {

        }

        public Rectangle(Vector2D loc1, Vector2D loc2)
        {
            X = Math.Min(loc1.X, loc2.X);
            Y = Math.Min(loc1.Y, loc2.Y);
            Width = Math.Max(loc1.X, loc2.X) - X;
            Height = Math.Max(loc1.Y, loc2.Y) - X;
        }

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Vector2D[] GetVertices()
        {
            Vector2D[] vertices = new Vector2D[4];

            vertices[0] = new Vector2D(Left, Top);
            vertices[1] = new Vector2D(Right, Top);
            vertices[2] = new Vector2D(Right, Bottom);
            vertices[3] = new Vector2D(Left, Bottom);

            return vertices;
        }

        public Rectangle GetBounds()
        {
            return this;
        }

        public FreeVector2D[] GetSides() => Polygon.GetSidesFromVertices(GetVertices());

        public bool Contains(Vector2D point) => (Left <= point.X && point.X <= Right && Bottom <= point.Y && point.Y <= Top);

        public Rectangle Offset(Vector2D delta)
        {
            return new Rectangle(X + delta.X, Y + delta.Y, Width, Height);
        }
        IShape IShape.Offset(Vector2D delta) => Offset(delta);

        public Polygon Rotate(double rad) => new Polygon(GetVertices()).Rotate(rad);
        IShape IShape.Rotate(double rad) => Rotate(rad);

        public Vector2D GetXFilteredPoint(Comparison<double> comparer) => new Polygon(GetVertices()).GetXFilteredPoint(comparer);

        public double GetParallelDistanceToPoint(Vector2D point, Vector2D direction) => new Polygon(GetVertices()).GetParallelDistanceToPoint(point, direction);

        IPolygon IPolygon.Offset(Vector2D delta) => Offset(delta);
        IPolygon IPolygon.Rotate(double rad) => Rotate(rad);
        public bool IntersectsWith(IPolygon polygon)
        {
            if(polygon is Rectangle)
            {
                Rectangle other = polygon as Rectangle;
                return this.Left < other.Right && 
                    this.Right > other.Left && 
                    this.Bottom < other.Top && 
                    this.Top > other.Bottom;
            } else return new Polygon(this.GetVertices()).IntersectsWith(polygon);
        }
        public Rectangle Intersect(Rectangle rectangle)
        {
            if (!IntersectsWith(rectangle)) return null;
            double x1 = Math.Max(this.Left, rectangle.Left);
            double y1 = Math.Max(this.Bottom, rectangle.Bottom);
            double x2 = Math.Min(this.Right, rectangle.Right);
            double y2 = Math.Min(this.Top, rectangle.Top);
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
        public IPolygon Intersect(IPolygon polygon)
        {
            if (polygon is Rectangle)
                return Intersect(polygon as Rectangle);
            else
                return new Polygon(this.GetVertices()).Intersect(polygon);
        }
        public FreeVector2D FlattenX() => new FreeVector2D(new Vector2D(Left, 0), new Vector2D(Right, 0));

        public override string ToString() => $"Rectangle[X={X},Y={Y},Width={Width},Height={Height}]";
        public override bool Equals(object obj) => Equals(obj as Rectangle);
        public bool Equals(Rectangle other) => other != null && X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

        public override int GetHashCode()
        {
            var hashCode = 466501756;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}
