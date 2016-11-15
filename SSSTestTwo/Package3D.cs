using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        
        public GeometryModel3D GetModel()
        {
            GeometryModel3D model = new GeometryModel3D();
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(0, 0, 0));
            mesh.Positions.Add(new Point3D(0, 1, 0));
            mesh.Positions.Add(new Point3D(0, 1, 1));
            mesh.Positions.Add(new Point3D(0, 0, 1));
            mesh.Positions.Add(new Point3D(1, 1, 0));
            mesh.Positions.Add(new Point3D(1, 1, 1));
            mesh.Positions.Add(new Point3D(1, 0, 0));
            mesh.Positions.Add(new Point3D(1, 0, 1));
            //Triangle 1
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);
            //Triangle 2
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            //Triangle 3  
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            //Triangle 4
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(5);
            //Triangle 5
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(0);
            //Triangle 6
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(3);
            //Triangle 7
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(2);
            //Triangle 8
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(5);
            //Triangle 9 - Back 
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(5);
            //Trangle 10 - Back  
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(7);
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(Brushes.Red);
            model.BackMaterial = new DiffuseMaterial(Brushes.CadetBlue);

            Transform3DGroup both = new Transform3DGroup();
            both.Children.Add(new ScaleTransform3D(Width, Height, Depth));
            both.Children.Add(new TranslateTransform3D(X, Y, Z));
            model.Transform = both;
            return model;
        }
    }
}
