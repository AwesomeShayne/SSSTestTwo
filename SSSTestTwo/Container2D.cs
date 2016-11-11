using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSSTestTwo
{
    class Container2D
    {
        public int Width;
        public int Height;
        public List<Bin2D> Bins;
        public List<Package2D> Packages;
        int packageCount = 0;

        public Container2D (int width, int height)
        {
            Width = width;
            Height = height;
            Bins = new List<Bin2D>();
            Bins.Add(new Bin2D(0, 0, width, height));
            Packages = new List<Package2D>();
        }

        public bool IsIntersect(Rectangle intersect)
        {
            if (intersect == Rectangle.Empty) return false;
            if (intersect.Width == 0 || intersect.Height == 0) return false;
            return true;
        }

        public bool AddBox(Package2D package)
        {
            bool collision = false;
            if (Bins.Count == 0) return false;
            foreach (Bin2D bin in Bins)
            {
                //Check if it fits...
                collision = false;
                //First move the box to the bin coords.
                package.X = bin.X;
                package.Y = bin.Y;

                //See if it fits in the container.
                if ((Width - package.X) >= package.Width && (Height - package.Y) >= package.Height)
                {
                    //Now see if it hits any other boxes we've already placed.
                    foreach (Package2D other in Packages)
                    {
                        Rectangle intersect = Rectangle.Intersect(package.ToRect(), other.ToRect());
                        collision = IsIntersect(intersect);
                        if (collision) break;
                    }
                    if (!collision) break;
                }
                //We hit something, let's rotate and try again.
                package.Rotate();
                if ((Width - package.X) >= package.Width && (Height - package.Y) >= package.Height)
                {
                    //Now see if it hits any other boxes we've already placed AGAIN.
                    foreach (Package2D other in Packages)
                    {
                        Rectangle intersect = Rectangle.Intersect(package.ToRect(), other.ToRect());
                        collision = IsIntersect(intersect);
                        if (collision) break;
                    }
                    //If we didn't hit any other boxes, the box can stay here.
                    if (!collision) break;
                }
                package.Rotate();
                //At this point, we must have hit something.
                collision = true;
                //We won't fit, so we go back to beginning and try the next bin.
            }
            //If we are still colliding, then it won't fit in this container.
            if (collision) return false;
            Packages.Add(package);
            //Since we are adding the box, we now need to split all of the bins it intersects.
            foreach (Bin2D bin in Bins.ToArray())
            {
                Bins.Remove(bin);
                Bins.AddRange(bin.Subtract(package.ToRect()));
            }
            packageCount++;
            package.Number = packageCount;
            return true;
        }

        public Bitmap BinImage()
        {
            int size = 5;
            var Output = new Bitmap(Width * size + size, Height * size + size);
            using (Graphics g = Graphics.FromImage(Output))
            {
                Random Rand = new Random();
                //g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, Width * 5, Height * 5));
                int _G;
                int _R;
                int _B;
                foreach (Package2D _Shape in Packages)
                {
                    var _Rect = new Rectangle(_Shape.X * size, _Shape.Y * size, _Shape.Width * size, _Shape.Height * size);
                    _R = Rand.Next(0, 256);
                    _G = Rand.Next(0, 256);
                    _B = Rand.Next(0, 256);
                    Color _FillColor = Color.FromArgb(size, _R, _G, _B);
                    Color _LineColor = Color.Black;
                    SolidBrush _FillBrush = new SolidBrush(_FillColor);
                    g.DrawString(_Shape.Number.ToString(), new Font("Tahoma", 6), Brushes.Black, _Rect.Location);
                    Pen _LineBrush = new Pen(_LineColor, 1);
                    g.FillRectangle(_FillBrush, _Rect);
                    g.DrawRectangle(_LineBrush, _Rect);
                }
                foreach (Bin2D _Bin in Bins)
                {
                    var _Rect = new Rectangle(_Bin.X * size, _Bin.Y * size, _Bin.Width * size, _Bin.Height * size);
                    g.DrawRectangle(Pens.Red, _Rect);
                }
            }
            return Output;
        }
    }
}
