using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace SSSTestTwo
{
    class Bin2D
    {
        private Rectangle _rect;

        public int Width { get { return _rect.Width; } set { _rect.Width = value; } }
        public int Height { get { return _rect.Height; } set { _rect.Height = value; } }
        public int X { get { return _rect.X; } set { _rect.X = value; } }
        public int Y { get { return _rect.Y; } set { _rect.Y = value; } }

        public Bin2D(Rectangle rect)
        {
            _rect = rect;
        }

        public Bin2D(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            _rect = rect;
        }

        public static Bin2D Create(int left, int right, int top, int bottom)
        {
            Rectangle rect = new Rectangle();
            rect.X = left;
            rect.Width = right - left;
            rect.Y = top;
            rect.Height = bottom - top;
            return new Bin2D(rect);
        }

        public Rectangle ToRect()
        {
            return _rect;
        }

        public int Area()
        {
            int area = _rect.Width * _rect.Height;
            if (area < 0)
                System.Diagnostics.Debugger.Break();
            return area;
        }

        public bool IsIntersect(Rectangle intersect)
        {
            if (intersect == Rectangle.Empty) return false;
            if (intersect.Width == 0 || intersect.Height == 0) return false;
            return true;
        }

        public List<Bin2D> Subtract(Rectangle sub)
        {
            Rectangle op = _rect;
            List<Bin2D> result = new List<Bin2D>();
            Rectangle intersect = Rectangle.Intersect(op, sub);
            if (!IsIntersect(intersect))
            {
                result.Add(new Bin2D(op));
            }
            else
            {
                int a = Math.Min(op.Left, intersect.Left);
                int b = Math.Max(op.Left, intersect.Left);
                int c = Math.Min(op.Right, intersect.Right);
                int d = Math.Max(op.Right, intersect.Right);

                int e = Math.Min(op.Top, intersect.Top);
                int f = Math.Max(op.Top, intersect.Top);
                int g = Math.Min(op.Bottom, intersect.Bottom);
                int h = Math.Max(op.Bottom, intersect.Bottom);

                // h +-+-+-+
                // . |5|6|7|
                // g +-+-+-+
                // . |3|X|4|
                // f +-+-+-+
                // . |0|1|2|
                // e +-+-+-+
                // . a b c d

                Bin2D bin;
                //Always have 1, 3, 4, and 6
                bin = Bin2D.Create(b, c, e, f); if (bin.Area() > 0) result.Add(bin);
                bin = Bin2D.Create(a, b, f, g); if (bin.Area() > 0) result.Add(bin);
                bin = Bin2D.Create(c, d, f, g); if (bin.Area() > 0) result.Add(bin);
                bin = Bin2D.Create(b, c, g, h); if (bin.Area() > 0) result.Add(bin);

                //decide on corners
                //if (op.Left == a && op.Top == e || intersect.Left == a && intersect.Top == e)
                //{ //corners 0 and 7
                bin = Bin2D.Create(a, b, e, f); if (bin.Area() > 0) result.Add(bin);
                bin = Bin2D.Create(c, d, g, h); if (bin.Area() > 0) result.Add(bin);
                //}
                //else
                //{ // corners 2 and 5
                bin = Bin2D.Create(c, d, e, f); if (bin.Area() > 0) result.Add(bin);
                bin = Bin2D.Create(a, b, g, h); if (bin.Area() > 0) result.Add(bin);
                //}
            }
            return result;
        }
    }
}
