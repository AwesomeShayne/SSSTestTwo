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
            GeometryModel3D Model = new GeometryModel3D();
            MeshGeometry3D Mesh = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(X,Y, Z));
            myPositionCollection.Add(new Point3D(X + Width,Y, Z));
            myPositionCollection.Add(new Point3D(X, Y + Height, Z));
            myPositionCollection.Add(new Point3D(X, Y, Z + Depth));
            myPositionCollection.Add(new Point3D(X + Width, Y + Height,Z));
            myPositionCollection.Add(new Point3D(X + Width, Y,Z + Depth));
            myPositionCollection.Add(new Point3D(X + Width, Y + Height, Z + Depth));
            Mesh.Positions = myPositionCollection;

            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));

            Mesh.TextureCoordinates = myTextureCoordinatesCollection;
            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(7);
            Mesh.TriangleIndices = myTriangleIndicesCollection;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            Model.Material = myMaterial;

            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.
            return Model;
        }
    }
}
