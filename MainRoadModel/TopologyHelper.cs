using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MainRoadModel
{
    /// <summary>
    /// Helps with topology operations
    /// </summary>
    public static class TopologyHelper
    {
        /// <summary>
        /// Point
        /// </summary>
    /*    public struct Point //Remove this class if you want to use System.Drawing.Point
        {
            private int x;
            private int y;

            public int X
            {
                get { return x; }
                set { x = value; }
            }

            public int Y
            {
                get { return y; }
                set { y = value; }
            }

            public Point(int x, int y)
            {
                this.x = 0;
                this.y = 0;
                X = x;
                Y = y;
            }

            public override int GetHashCode()
            {
                return x ^ y;
            }

            public override bool Equals(object obj)
            {
                var other = (Point)obj;
                return other.x == x && other.y == y;
            }
        }
        */

        public struct ROI : IEnumerable<Point>
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public IEnumerator<Point> GetEnumerator()
            {
                var toX = X + Width;
                var toY = Y + Height;

                for (int y = Y; y < toY; y++)
                for (int x = X; x < toX; x++)
                    yield return new Point(x, y);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public ROI(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }


            public ROI Inflate(int dx, int dy)
            {
                return new ROI(X + dx, Y + dy, Math.Max(0, Width - dx * 2), Math.Max(0, Height - dy * 2));
            }

            public IEnumerable<Point> Border()
            {
                for (int x = X; x < X + Width; x++)
                    yield return new Point(x, Y);
                for (int x = X; x < X + Width; x++)
                    yield return new Point(x, Y + Height - 1);

                for (int y = Y; y < Y + Height; y++)
                    yield return new Point(X, y);
                for (int y = Y; y < Y + Height; y++)
                    yield return new Point(X + Width - 1, y);
            }
        }

        /// <summary>
        /// Is this point filled?
        /// <remark>This is default function used when you do not pass function into methods.</remark>
        /// </summary>
        public static Func<Point, bool> IsFilled;

        public static IEnumerable<Point> GetNeighbors(this Point point, TopType topologyType = TopType.N8)
        {
            switch(topologyType)
            {
                case TopType.N4:
                case TopType.N4AndMe:
                    yield return new Point(point.X + 0, point.Y - 1);
                    yield return new Point(point.X + 1, point.Y + 0);
                    yield return new Point(point.X + 0, point.Y + 1);
                    yield return new Point(point.X - 1, point.Y + 0);
                    if (topologyType == TopType.N8AndMe)
                        yield return point;
                    break;
                case TopType.N8:
                case TopType.N8AndMe:
                    yield return new Point(point.X - 1, point.Y - 1);
                    yield return new Point(point.X + 0, point.Y - 1);
                    yield return new Point(point.X + 1, point.Y - 1);
                    yield return new Point(point.X + 1, point.Y + 0);
                    yield return new Point(point.X + 1, point.Y + 1);
                    yield return new Point(point.X + 0, point.Y + 1);
                    yield return new Point(point.X - 1, point.Y + 1);
                    yield return new Point(point.X - 1, point.Y + 0);
                    if (topologyType == TopType.N8AndMe)
                        yield return point;
                    break;
            }
        }

        public static IEnumerable<Point> GetCorners(this Point point)
        {
            yield return new Point(point.X - 1, point.Y - 1);
            yield return new Point(point.X + 1, point.Y - 1);
            yield return new Point(point.X + 1, point.Y + 1);
            yield return new Point(point.X - 1, point.Y + 1);
        }

        /// <summary>
        /// Returns code with info of filled neighbors. Digit is 1 if cell is filled, 0 - not filled. Digit positions:
        /// 
        /// 0 1 2
        /// 7 X 3
        /// 6 5 4   - for 8-connected topology
        /// 
        ///   0  
        /// 3 X 1
        ///   2     - for 4-connected topology
        /// 
        /// <remark>For example: if you use 4-connected topology and top cell and bottom cell is filled, then method returns code 1010</remark>>
        /// </summary>
        public static int GetFilledNeighborsCode(this Point point, Func<Point, bool> isFilled = null, TopType topologyType = TopType.N8)
        {
            var res = 0;
            foreach(var p in point.GetNeighbors(topologyType))
            {
                res *= 10;
                if ((isFilled ?? IsFilled)(p))
                    res = res + 1;
            }

            return res;
        }

        public static bool IsBorder(this Point point, Func<Point, bool> isFilled = null, TopType topologyType = TopType.N8)
        {
            return (isFilled ?? IsFilled)(point) && point.GetNeighbors(topologyType).Count(isFilled ?? IsFilled) < (int)topologyType;
        }

        public static IEnumerable<Point> GetBorder(this IEnumerable<Point> cells, Func<Point, bool> isFilled = null, TopType topologyType = TopType.N8)
        {
            return cells.Where(c => c.IsBorder(isFilled ?? IsFilled, topologyType));
        }

        public static bool IsOutsideBorder(this Point point, Func<Point, bool> isFilled = null, TopType topologyType = TopType.N8)
        {
            return !(isFilled ?? IsFilled)(point) && point.GetNeighbors(topologyType).Count(isFilled ?? IsFilled) > 0;
        }

        public static IEnumerable<Point> GetOutsideBorder(this IEnumerable<Point> cells, Func<Point, bool> isFilled = null, TopType topologyType = TopType.N8)
        {
            return cells.Where(c => c.IsOutsideBorder(isFilled ?? IsFilled, topologyType));
        }

        public static bool IsInside(this IEnumerable<Point> cells1, IEnumerable<Point> cells2)
        {
            var hash = new HashSet<Point>(cells1);
            return cells2.All(hash.Contains);
        }
    }

    public enum TopType
    {
        N8 = 8,
        N8AndMe = 9,
        N4 = 4,
        N4AndMe = 5
    }
}