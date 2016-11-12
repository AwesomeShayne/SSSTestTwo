using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Threading.Tasks;

namespace SSSTestTwo
{
    class Bin3D
    {
        private Rect3D _rect;

        public int Width { get { return (int)_rect.SizeX; } set { _rect.SizeX = value; } }
        public int Depth { get { return (int)_rect.SizeZ; } set { _rect.SizeZ = value; } }
        public int Height { get { return (int)_rect.SizeY; } set { _rect.SizeY = value; } }

        public int X { get { return (int)_rect.X; } set { _rect.X = value; } }
        public int Y{ get { return (int)_rect.Y; } set { _rect.Y = value; } }
        public int Z { get { return (int)_rect.Z; } set { _rect.Z = value; } }

        public Bin3D(Rect3D rect)
        {
            _rect = rect;
        }

        public Bin3D(int x, int y, int z, int width, int height, int depth)
        {
            Rect3D rect = new Rect3D(x, y, z, width, height, depth);
            _rect = rect;
        }

        public static Bin3D Create(int left, int right, int top, int bottom, int front, int back)
        {
            try
            {
                Rect3D rect = new Rect3D();
                rect.X = left;
                rect.SizeX = right - left;
                rect.Y = bottom;
                rect.SizeY = top - bottom;
                rect.Z = front;
                rect.Z = back - front;
                return new Bin3D(rect);
            }
            catch (Exception e)
            {
                return new Bin3D(0, 0, 0, 0, 0, 0);
            }
        }

        public Rect3D ToRect()
        {
            return _rect;
        }

        public int Volume()
        {
            int volume = (int)_rect.SizeX * (int)_rect.SizeY * (int)_rect.SizeZ;
            if (volume < 0)
                System.Diagnostics.Debugger.Break();
            return volume;
        }

        public bool IsIntersect(Rect3D intersect)
        {
            if (intersect == Rect3D.Empty) return false;
            if (intersect.SizeX == 0 || intersect.SizeY == 0 || intersect.SizeZ == 0) return false;
            return true;
        }

        public List<Bin3D> Subtract(Rect3D sub)
        {
            Rect3D op = _rect;
            List<Bin3D> result = new List<Bin3D>();
            Rect3D intersect = Rect3D.Intersect(op, sub);
            if (!IsIntersect(intersect))
            {
                result.Add(new Bin3D(op));
            }
            else
            {
                int a = Math.Min((int)op.X, (int)intersect.X);
                int b = Math.Max((int)op.X, (int)intersect.X);
                int c = Math.Min((int)op.X + (int)op.SizeX, (int)intersect.X + (int)intersect.SizeX);
                int d = Math.Max((int)op.X + (int)op.SizeX, (int)intersect.X + (int)intersect.SizeX);

                int e = Math.Min((int)op.Y, (int)intersect.Y);
                int f = Math.Max((int)op.Y, (int)intersect.Y);
                int g = Math.Min((int)op.Y + (int)op.SizeY, (int)intersect.Y + (int)intersect.SizeY);
                int h = Math.Max((int)op.Y + (int)op.SizeY, (int)intersect.Y + (int)intersect.SizeY);

                int i = Math.Min((int)op.Z, (int)intersect.Z);
                int j = Math.Max((int)op.Z, (int)intersect.Z);
                int k = Math.Min((int)op.Z + (int)op.SizeZ, (int)intersect.Z + (int)intersect.SizeZ);
                int l = Math.Max((int)op.Z + (int)op.SizeZ, (int)intersect.Z + (int)intersect.SizeZ);

                /*          _   _   _   l
                 *        /   /   /   /
                 *       / _ / _ / _ / k
                 *      /   /   /   /
                 *     / _ / _ / _ / j
                 *    /   /   /   /
                 * h +___+___+___+ i
                 * . |   |   |   |
                 * g +___+___+___+
                 * . |   |   |   |
                 * f +___+___+___+
                 * . |   |   |   |
                 * e +___+___+___+
                 *   a   b   c   d
                 * 
                 * 
                 */
                Bin3D bin;
                bin = Create(b, c, e, f, i, j); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, f, g, i, j); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(c, d, f, g, i, j); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(b, c, g, h, i, j); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(a, b, f, g, j, k); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(b, c, e, f, j, k); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(b, c, g, h, j, k); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(c, d, f, g, j, k); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(b, c, e, f, k, l); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, f, g, k, l); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(c, d, f, g, k, l); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(b, c, g, h, k, l); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(c, d, g, h, i, j); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, e, f, i, j); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(c, d, g, h, j, k); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, e, f, j, k); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(c, d, g, h, k, l); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, e, f, k, l); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(c, d, e, f, i, j); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, g, h, i, j); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(c, d, e, f, j, k); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, g, h, j, k); if (bin.Volume() > 0) result.Add(bin);

                bin = Create(c, d, e, f, k, l); if (bin.Volume() > 0) result.Add(bin);
                bin = Create(a, b, g, h, k, l); if (bin.Volume() > 0) result.Add(bin);
            }
            return result;
        }
    }
}
