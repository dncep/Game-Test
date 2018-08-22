using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Test.Util
{
    public interface IShape
    {
        bool Contains(Vector2D point);
        IShape Offset(Vector2D delta);
        IShape Rotate(double rad);
        double GetParallelDistanceToPoint(Vector2D point, Vector2D direction);
    }

    public interface IPolygon : IShape
    {
        new IPolygon Offset(Vector2D delta);
        new IPolygon Rotate(double rad);

        Vector2D[] GetVertices();
        FreeVector2D[] GetSides();
        Vector2D GetXFilteredPoint(Comparison<double> comparer);
        bool IntersectsWith(IPolygon polygon);

        FreeVector2D FlattenX();

        Rectangle GetBounds();
        IPolygon Intersect(IPolygon other);
    }

    public class Circle : IShape
    {
        public Vector2D Center;
        public double Radius;

        public Circle(Vector2D center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Contains(Vector2D point) => (point - Center).Magnitude <= Radius;

        public Circle Offset(Vector2D delta) => new Circle(Center + delta, Radius);
        IShape IShape.Offset(Vector2D delta) => Offset(delta);

        public Circle Rotate(double rad) => new Circle(Center.Rotate(rad), Radius);
        IShape IShape.Rotate(double rad) => Rotate(rad);

        public double GetParallelDistanceToPoint(Vector2D point, Vector2D direction) => throw new NotImplementedException();
    }

    public class Polygon : IPolygon, IEquatable<Polygon>
    {
        private readonly Vector2D[] vertices;
        private readonly Rectangle bounds;

        public Polygon(params Vector2D[] vertices)
        {
            this.vertices = vertices;

            double minX = 0;
            double minY = 0;
            double maxX = 0;
            double maxY = 0;
            bool first = true;

            foreach(Vector2D vertex in vertices)
            {
                if(first)
                {
                    minX = maxX = vertex.X;
                    minY = maxY = vertex.Y;
                    first = false;
                } else
                {
                    minX = Math.Min(minX, vertex.X);
                    minY = Math.Min(minY, vertex.Y);
                    maxX = Math.Max(maxX, vertex.X);
                    maxY = Math.Max(maxY, vertex.Y);
                }
            }

            this.bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public Vector2D[] GetVertices()
        {
            return vertices;
        }

        public FreeVector2D[] GetSides()
        {
            return GetSidesFromVertices(vertices);
        }

        public bool Contains(Vector2D point)
        {
            if (!bounds.Contains(point)) return false;
            int hits = 0;
            FreeVector2D scanline = new FreeVector2D(point, point + new Vector2D(bounds.Width, 0));
            foreach (FreeVector2D side in GetSides())
            {
                if (side.Contains(point)) return true;
                if (side.GetIntersection(scanline) != null) hits++;
            }
            return hits % 2 != 0;
        }

        public Polygon Rotate(double rad)
        {
            Vector2D[] rotated = new Vector2D[vertices.Length];
            for(int i = 0; i < vertices.Length; i++)
            {
                rotated[i] = vertices[i].Rotate(rad);
            }
            return new Polygon(rotated);
        }

        IShape IShape.Rotate(double rad) => Rotate(rad);
        IPolygon IPolygon.Rotate(double rad) => Rotate(rad);

        public Polygon Offset(Vector2D delta)
        {
            Vector2D[] moved = new Vector2D[vertices.Length];
            for(int i = 0; i < vertices.Length; i++)
            {
                moved[i] = vertices[i] + delta;
            }
            return new Polygon(moved);
        }

        IShape IShape.Offset(Vector2D delta) => Offset(delta);
        IPolygon IPolygon.Offset(Vector2D delta) => Offset(delta);

        public Vector2D GetXFilteredPoint(Comparison<double> comparer)
        {
            Vector2D rightmost = vertices[0];
            foreach(Vector2D vertex in vertices)
            {
                int result = comparer(rightmost.X, vertex.X);

                if (result < 0) rightmost = vertex;
                else if (result == 0) rightmost = (rightmost + vertex) / 2;
            }
            return rightmost;
        }

        public double GetParallelDistanceToPoint(Vector2D point, Vector2D direction)
        {
            //Rotate both the point and the polygon to be horizontal with the direction
            point = point.Rotate(-direction.Angle);
            Polygon rotated = this.Rotate(-direction.Angle);

            //Sort by horizontal distance to rotated point
            Vector2D tip = GetXFilteredPoint((a, b) => (int)((Math.Abs(b - point.X) - Math.Abs(a - point.X)) * 1000));

            return Math.Abs(tip.X - point.X);
        }

        public static FreeVector2D[] GetSidesFromVertices(Vector2D[] vertices)
        {
            FreeVector2D[] sides = new FreeVector2D[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i < vertices.Length - 1) sides[i] = new FreeVector2D(vertices[i], vertices[i + 1]);
                else sides[i] = new FreeVector2D(vertices[i], vertices[0]);
            }
            return sides;
        }

        public FreeVector2D FlattenX()
        {
            FreeVector2D flattened = new FreeVector2D(
                this.GetXFilteredPoint((a, b) => (int)((a - b) * 1000)),
                this.GetXFilteredPoint((a, b) => (int)((b - a) * 1000)));
            flattened.A.Y = 0;
            flattened.B.Y = 0;

            if(flattened.A.X > flattened.B.X)
            {
                flattened.SwapEnds();
            }
            return flattened;
        }

        public bool IntersectsWith(IPolygon polygon)
        {
            //Make sure to perform SAT on the polygon with fewer sides
            if (polygon.GetSides().Length < (this.vertices.Length)) return polygon.IntersectsWith(this);

            FreeVector2D[] sides = this.GetSides();
            Vector2D[] normals = new Vector2D[sides.Length];

            for(int i = 0; i < sides.Length; i++)
            {
                normals[i] = sides[i].Normal;
            }

            foreach(Vector2D axis in normals)
            {
                FreeVector2D thisRotated = this.Rotate(-axis.Angle).FlattenX();
                FreeVector2D otherRotated = polygon.Rotate(-axis.Angle).FlattenX();

                if (Math.Min(thisRotated.B.X, otherRotated.B.X) <= Math.Max(thisRotated.A.X, otherRotated.A.X))
                {
                    //Projection shows no intersections.
                    return false;
                }
            }
            //No intersections found.
            return true;
        }

        public bool IntersectsWith(FreeVector2D line)
        {
            Polygon thisRotated = this.Rotate(-line.Angle);
            FreeVector2D lineRotated = line.Rotate(-line.Angle);
            
            if (thisRotated.Contains(lineRotated.A)) return true;
            if (thisRotated.Contains(lineRotated.B)) return true;

            foreach (FreeVector2D side in thisRotated.GetSides())
            {
                if (side.IntersectsWith(lineRotated)) return true;
            }
            return false;
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public Polygon Intersect(IPolygon polygon)
        {
            Polygon other = (polygon is Polygon) ? polygon as Polygon : new Polygon(polygon.GetVertices());
            
            foreach (FreeVector2D side in this.GetSides())
            {
                if (other is null) return null;
                other = ComputeLineClip(other, side);
            }

            return other;
        }

        private static Polygon ComputeLineClip(Polygon polygon, FreeVector2D line)
        {
            polygon = polygon.Offset(-line.A).Rotate(-line.Angle);
            // By now, anything y > 0 would be above the clipping line

            List<Vector2D> vertices = polygon.GetVertices().ToList();

            //Trim unnecessary clipped vertices.

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2D previousVertex = vertices[GeneralUtil.FloorMod(i - 1, vertices.Count)];
                Vector2D nextVertex = vertices[GeneralUtil.FloorMod(i + 1, vertices.Count)];

                Vector2D vertex = vertices[i];

                if (previousVertex.Y > 0 && vertex.Y > 0 && nextVertex.Y > 0)
                {
                    vertices.RemoveAt(i);
                    i--;
                }
            }

            if (vertices.Count == 0) return null;
            
            for(int i = 0; i < vertices.Count; i++)
            {
                Vector2D previousVertex = vertices[GeneralUtil.FloorMod(i-1, vertices.Count)];
                Vector2D nextVertex = vertices[GeneralUtil.FloorMod(i+1, vertices.Count)];

                Vector2D vertex = vertices[i];

                if(vertex.Y > 0)
                {
                    Vector2D newVertex0 = (previousVertex.Y <= 0) ? GetYZeroIntersection(previousVertex, vertex) : GetYZeroIntersection(vertices[GeneralUtil.FloorMod(i - 2, vertices.Count)], previousVertex);
                    Vector2D newVertex1 = (nextVertex.Y <= 0) ? GetYZeroIntersection(vertex, nextVertex) : GetYZeroIntersection(nextVertex, vertices[GeneralUtil.FloorMod(i + 2, vertices.Count)]);

                    vertices.RemoveAt(i);
                    if (!vertices.Contains(newVertex0))
                    {
                        vertices.Insert(i, newVertex0);
                        i++;
                    }
                    if (!vertices.Contains(newVertex1))
                    {
                        vertices.Insert(i, newVertex1);
                    }
                    i--;
                }
            }

            //Rotate back and round
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = (vertices[i].Rotate(line.Angle) + line.A).GetRounded();
            }

            polygon = new Polygon(vertices.Distinct().ToArray());
            return polygon;
        }

        public static Vector2D GetYZeroIntersection(Vector2D a, Vector2D b)
        {
            double xOff = 0;
            if (a.X != b.X && a.Y != b.Y)
            {
                xOff = -a.Y / Math.Tan((b - a).Angle);
            }
            return new Vector2D(a.X + xOff, 0);
        }


        IPolygon IPolygon.Intersect(IPolygon other) => Intersect(other);

        public static Polygon CreatePolygonFromSides(List<FreeVector2D> sides)
        {
            List<Vector2D> vertices = new List<Vector2D>();
            
            FreeVector2D previousSide = sides[0];
            Vector2D current = previousSide.B;
            vertices.Add(previousSide.A);
            sides.RemoveAt(0);

            while(sides.Count > 0)
            {
                for (int i = 0; i < sides.Count; i++)
                {
                    FreeVector2D side = sides[i];
                    if (side.A.Equals(current))
                    {
                        vertices.Add(current);
                        current = side.B;
                        sides.RemoveAt(i);
                        break;

                    }
                    else if (side.B.Equals(current))
                    {
                        vertices.Add(current);
                        current = side.A;
                        sides.RemoveAt(i);
                        break;
                    }

                    if (i == sides.Count - 1) throw new ArgumentException("Unclosed polygon");
                }
            }

            return new Polygon(vertices.ToArray());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            bool empty = true;
            foreach(Vector2D vertex in vertices)
            {
                empty = false;
                sb.Append(vertex);
                sb.Append(", ");
            }
            if (!empty) sb.Length -= 2;
            sb.Append('}');
            return sb.ToString();
        }

        public override bool Equals(object obj) => Equals(obj as Polygon);
        public bool Equals(Polygon other)
        {
            if (other == null) return false;
            if (this.vertices.Length != other.vertices.Length) return false;

            return Enumerable.SequenceEqual(vertices.OrderBy(v => v.GetHashCode()), other.vertices.OrderBy(v => v.GetHashCode()));
        }
        public override int GetHashCode()
        {
            int hash = 911371710;
            foreach(Vector2D vertex in vertices)
            {
                hash += vertex.GetHashCode();
            }
            return hash;
        }
    }
}
