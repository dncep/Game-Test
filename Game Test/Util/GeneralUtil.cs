using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Test.Util
{
    public sealed class GeneralUtil
    {
        public static string EnumerableToString<T>(IEnumerable<T> arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            bool empty = true;
            foreach (T obj in arr)
            {
                empty = false;
                sb.Append(obj);
                sb.Append(", ");
            }
            if (!empty) sb.Length -= 2;
            sb.Append(']');
            return sb.ToString();
        }

        public static float FloorMod(float x, float y)
        {
            float r = Math.Abs(x) % Math.Abs(y);
            r *= Math.Sign(x);
            r = (r + Math.Abs(y)) % Math.Abs(y);
            return r;
        }

        public static double FloorMod(double x, double y)
        {
            double r = Math.Abs(x) % Math.Abs(y);
            r *= Math.Sign(x);
            r = (r + Math.Abs(y)) % Math.Abs(y);
            return r;
        }

        public static int FloorMod(int x, int y)
        {
            int r = Math.Abs(x) % Math.Abs(y);
            r *= Math.Sign(x);
            r = (r + Math.Abs(y)) % Math.Abs(y);
            return r;
        }
    }
}
