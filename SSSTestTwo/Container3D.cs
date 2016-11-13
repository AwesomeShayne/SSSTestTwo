using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SSSTestTwo
{
    class Container3D
    {
        public int Width;
        public int Height;
        public int Depth;
        
        public List<Bin3D> Bins;
        public List<Package3D> Packages;
        int packageCount = 0;

        public Container3D(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Bins = new List<Bin3D>();
            Bins.Add(new Bin3D(0, 0, 0, width, height, depth));
            Packages = new List<Package3D>();
        }

        public bool IsIntersect(Rect3D intersect)
        {
            if (intersect == Rect3D.Empty) return false;
            if (intersect.SizeX == 0 || intersect.SizeY == 0 || intersect.SizeZ == 0) return false;
            return true;
        }

        public bool AddBox(Package3D package)
        {
            bool collision = false;
            foreach (Bin3D bin in Bins)
            {
                // Ensure that we start by assuming that it isn't colliding
                collision = false;
                // Move the box to the bin coordinates
                package.X = bin.X;
                package.Y = bin.Y;
                package.Z = bin.Z;
                // Does it fit?
                if ((Width - package.X) >= package.Width && (Height - package.Y) >= package.Height && (Depth - package.Z) >= package.Depth)
                {
                    //Does it collide?
                    foreach (Package3D other in Packages)
                    {
                        Rect3D intersect = Rect3D.Intersect(package.ToRect(), other.ToRect());
                        collision = IsIntersect(intersect);
                        if (collision) break;
                    }
                    if (!collision) break;
                }
            }
            // if it won't fit in the container GTFO
            if (collision) return false;
            Packages.Add(package);
            // Now we break apart the bins
            foreach (Bin3D bin in Bins.ToArray())
            {
                Bins.Remove(bin);
                Bins.AddRange(bin.Subtract(package.ToRect()));
            }
            packageCount++;
            package.Number = packageCount;
            return true;
        }

        public ModelVisual3D BinImage()
        {
            Model3DGroup myModel3DGroup = new Model3DGroup();
            
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view 
            // of the camera.           

            // Asign the camera to the viewport
            // Define the lights cast in the scene. Without light, the 3D object cannot 
            // be seen. Note: to illuminate an object from additional directions, create 
            // additional lights.

            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);

            myModel3DGroup.Children.Add(myDirectionalLight);

            // Add the geometry model to the model group.
            foreach (Package3D Box in Packages)
            {
                myModel3DGroup.Children.Add(Box.GetModel());
            }

            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;
            return myModelVisual3D;
            // 
        }

    }
}
