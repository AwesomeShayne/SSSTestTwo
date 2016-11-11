using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SSSTestTwo
{
    class Package2D
    {
        private Rectangle _rect;
        public int Width { get { return _rect.Width; } set { _rect.Width = value; } }
        public int Height { get { return _rect.Height; } set { _rect.Height = value; } }
        public int X { get { return _rect.X; } set { _rect.X = value; } }
        public int Y { get { return _rect.Y; } set { _rect.Y = value; } }
        public int Number;

        public Package2D(Rectangle rect)
        {
            _rect = rect;
        }

        public Package2D(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            _rect = rect;
        }

        public Rectangle ToRect()
        {
            return _rect;
        }

        public int Area()
        {
            return _rect.Width * _rect.Height;
        }

        public void Rotate()
        {
            int temp = _rect.Width;
            _rect.Width = _rect.Height;
            _rect.Height = temp;
        }
    }
}
