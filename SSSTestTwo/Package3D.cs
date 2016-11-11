using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SSSTestTwo
{
    class Package3D
    {
        private Rect3D _rect;
        public int Width { get { return (int)_rect.SizeX; } set { _rect.SizeX = value; } }
        public int Depth { get { return (int)_rect.SizeZ; } set { _rect.SizeZ = value; } }
        public int Height { get { return (int)_rect.SizeY; } set { _rect.SizeY = value; } }

        public int X { get { return (int)_rect.X; } set { _rect.X = value; } }
        public int Y { get { return (int)_rect.Y; } set { _rect.Y = value; } }
        public int Z { get { return (int)_rect.Z; } set { _rect.Z = value; } }

        public int Number;

        public Package3D(Rect3D rect)
        {
            _rect = rect;
        }
        public Package3D(int x, int y, int z, int width, int height, int depth)
        {
            Rect3D rect = new Rect3D(x, y, z, width, height, depth);
            _rect = rect;
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
        
    }
}
