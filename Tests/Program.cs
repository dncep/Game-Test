using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Game_Test.Util;
using System.Text;

namespace Tests
{
    class Test : Attribute
    {
    }

    class Solo : Attribute
    {
    }

    class Program
    {
        public delegate void TestDelegate();

        static List<MethodInfo> possibleTests;
        static Dictionary<MethodInfo, Action> tests;

        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            possibleTests = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(Test), false).Length > 0)
                      .ToList();
            
            if(possibleTests.Any(m => m.GetCustomAttributes(typeof(Solo), false).Length > 0))
            {
                possibleTests.RemoveAll(m => m.GetCustomAttributes(typeof(Solo), false).Length <= 0);
            }

            tests = possibleTests.ToDictionary(t => t, t => (Action)t.CreateDelegate(typeof(Action)));

            int passed = 0;
            int failed = 0;
            int total = tests.Count;
            foreach(KeyValuePair<MethodInfo, Action> test in tests)
            {
                try
                {
                    //Console.WriteLine($"Testing '{test.Key}'...");
                    test.Value();
                    passed++;
                } catch(AssertionError x)
                {
                    Console.WriteLine("Failed test " + test.Key.Name + ":\n\t" + x.Message);
                    failed++;
                }
            }

            Console.WriteLine($"Finished {total} tests: {passed} passed, {failed} failed");
            Console.ReadLine();
        }

        [Test]
        public static void VectorRotation()
        {
            Vector2D vec = new Vector2D(0, -5);

            Assert(vec.Rotate(-Math.PI/2), new Vector2D(-5, 0));
        }

        [Test]
        public static void VectorAngle()
        {
            Vector2D vec = new Vector2D(0, 5);

            Assert(vec.Angle, Math.PI / 2);
        }

        [Test]
        public static void VectorMagnitude()
        {
            Vector2D vec = new Vector2D(0, 5);

            Assert(vec.Magnitude, 5d);
        }

        [Test]
        public static void FreeVectorRotation()
        {
            FreeVector2D vec = new FreeVector2D(new Vector2D(0, -5), new Vector2D(0, 5));

            Assert(vec.Rotate(-Math.PI / 2), new FreeVector2D(new Vector2D(-5, 0), new Vector2D(5, 0)));
        }

        [Test]
        public static void VectorIntersection()
        {
            FreeVector2D vec1 = new FreeVector2D(new Vector2D(-1, -5), new Vector2D(1, 5));
            FreeVector2D vec2 = new FreeVector2D(new Vector2D(-5, -5), new Vector2D(5, 5));

            Assert(vec1.GetIntersection(vec2), new Vector2D(0, 0));
        }

        [Test]
        public static void RectangleIntersectionMiss()
        {
            Polygon polygon1 = new Polygon(new Rectangle(8, 0, 16, 16).GetVertices());
            Polygon polygon2 = new Polygon(new Rectangle(8, 17, 16, 16).GetVertices());

            Assert(polygon1.IntersectsWith(polygon2), false);
        }

        [Test]
        public static void RectangleIntersectionHit()
        {
            Polygon polygon1 = new Polygon(new Rectangle(8, 0, 16, 16).GetVertices());
            Polygon polygon2 = new Polygon(new Rectangle(4, 7, 16, 16).GetVertices());

            Assert(polygon1.IntersectsWith(polygon2), true);
        }

        [Test]
        public static void TriangleRectangleIntersectionMiss()
        {
            Polygon polygon1 = new Polygon(new Vector2D(), new Vector2D(-1, 2), new Vector2D(5, 0));
            Polygon polygon2 = new Polygon(new Rectangle(3, 1, 4, 4).GetVertices());

            Assert(polygon1.IntersectsWith(polygon2), false);
        }

        [Test]
        public static void TriangleRectangleIntersectionHit()
        {
            Polygon polygon1 = new Polygon(new Vector2D(), new Vector2D(-1, 2), new Vector2D(5, 0));
            Polygon polygon2 = new Polygon(new Rectangle(1, 1, 4, 4).GetVertices());

            Assert(polygon1.IntersectsWith(polygon2), true);
        }

        [Test]
        public static void PolygonLineIntersectionMiss()
        {
            Polygon polygon = new Polygon(new Vector2D(), new Vector2D(-1, 2), new Vector2D(5, 0));
            FreeVector2D line = new FreeVector2D(new Vector2D(-1, 3), new Vector2D(5, 1));

            Assert(polygon.IntersectsWith(line), false);
        }

        [Test]
        public static void PolygonLineIntersectionHit()
        {
            Polygon polygon = new Polygon(new Vector2D(), new Vector2D(-1, 2), new Vector2D(5, 0));
            FreeVector2D line = new FreeVector2D(new Vector2D(-1, 3), new Vector2D(5, -1));

            Assert(polygon.IntersectsWith(line), true);
        }

        [Test]
        public static void FlatRectangleContains()
        {
            Rectangle rect = new Rectangle(0, 0, 4, 0);
            Vector2D point = new Vector2D(4, 0);

            Assert(rect.Contains(point), true);
        }

        [Test]
        public static void VertexIntersection()
        {
            FreeVector2D line1 = new FreeVector2D(new Vector2D(), new Vector2D(4, 0));
            FreeVector2D line2 = new FreeVector2D(new Vector2D(0, 4), new Vector2D(4, 0));

            Assert(line1.GetIntersection(line2), new Vector2D(4, 0));
        }

        [Test]
        public static void PolygonBounds()
        {
            Polygon polygon = new Polygon(new Vector2D(), new Vector2D(0, 4), new Vector2D(4, 0));

            Assert(polygon.GetBounds(), new Rectangle(0, 0, 4, 4));
        }

        [Test]
        public static void PolygonVertexContained()
        {
            Polygon polygon = new Polygon(new Vector2D(), new Vector2D(0, 4), new Vector2D(4, 0));
            Vector2D vertex = new Vector2D(0, 4);

            Assert(polygon.Contains(vertex), true);
        }

        [Test]
        public static void PolygonIntersection()
        {
            Polygon polygon1 = new Polygon(new Vector2D(), new Vector2D(0, 4), new Vector2D(4, 0));
            Polygon polygon2 = new Polygon(new Vector2D(), new Vector2D(4, 4), new Vector2D(4, 0));

            Assert(polygon1.Intersect(polygon2), new Polygon(new Vector2D(0, 0), new Vector2D(2, 2), new Vector2D(4, 0)));
        }

        [Test]
        public static void RectangleIntersectTrue()
        {
            Rectangle rect1 = new Rectangle(-8, -8, 16, 16);
            Rectangle rect2 = new Rectangle(0, 0, 16, 16);

            Assert(rect1.IntersectsWith(rect2), true);
        }

        [Test]
        public static void RectangleIntersectFalse()
        {
            Rectangle rect1 = new Rectangle(-8, -8, 16, 16);
            Rectangle rect2 = new Rectangle(0, 9, 16, 16);

            Assert(rect1.IntersectsWith(rect2), false);
        }

        [Test]
        public static void RectangleIntersection()
        {
            Rectangle rect1 = new Rectangle(-8, -8, 16, 16);
            Rectangle rect2 = new Rectangle(0, 0, 16, 16);

            Assert(rect1.Intersect(rect2), new Rectangle(0, 0, 8, 8));
        }

        [Test]
        public static void RectangleTriangleIntersection1()
        {
            Polygon polygon = new Polygon(new Vector2D(8, 8), new Vector2D(8, 24), new Vector2D(24, 8));
            Rectangle rect = new Rectangle(0, 21.7895311181912, 16, 16);
            
            Assert(rect.Intersect(polygon), new Polygon(new Vector2D(10.2104689, 21.7895311), new Vector2D(8, 21.7895311), new Vector2D(8, 24)));
        }

        [Test]
        public static void RectangleTriangleIntersection2()
        {
            Polygon polygon = new Polygon(new Vector2D(-8, 8), new Vector2D(-8, 24), new Vector2D(8, 8));
            Rectangle rect = new Rectangle(-8, 7.99999999653196, 16, 16);

            Assert(rect.Intersect(polygon), new Polygon(new Vector2D(-8, 8), new Vector2D(-8, 24), new Vector2D(8, 8)));
        }

        [Test]
        public static void RectangleTriangleIntersection3()
        {
            Polygon polygon = new Polygon(new Vector2D(8, 8), new Vector2D(-8, 8), new Vector2D(8, 24));
            Rectangle rect = new Rectangle(-8, -8, 16, 16);

            Assert(polygon.Intersect(rect), new Polygon(new Vector2D(-8, 8), new Vector2D(8, 8)));
        }

        [Test]
        public static void FloorModTest()
        {
            Assert(GeneralUtil.FloorMod(-1, 1), 0);
            Assert(GeneralUtil.FloorMod(-2, 1), 0);
            Assert(GeneralUtil.FloorMod(-1, 2), 1);
            Assert(GeneralUtil.FloorMod(0, 2), 0);
        }

        public static void Assert(object real, object expected)
        {
            if (!Equals(real, expected))
            {
                throw new AssertionError($"Expected {expected}, instead got {real}");
            }
        }
    }
}
