using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Wpf3D.Controls
{
    internal static class Mesh3DFactory
    {
        public static MeshGeometry3D CreateXZPlane(double xLength, double zLength, bool freeze = true)
        {
            var mesh = new MeshGeometry3D
            {
                Positions = new Point3DCollection
                {
                    new Point3D(0, 0, 0),
                    new Point3D(xLength, 0, 0),
                    new Point3D(xLength, 0, -zLength),
                    new Point3D(0, 0, -zLength)
                },
                Normals = new Vector3DCollection
                {
                    new Vector3D(0, 1, 0),
                    new Vector3D(0, 1, 0),
                    new Vector3D(0, 1, 0),
                    new Vector3D(0, 1, 0)
                },
                TriangleIndices = new Int32Collection { 0, 1, 2, 0, 2, 3 }
            };

            if (freeze && mesh.CanFreeze)
            {
                mesh.Freeze();
            }

            return mesh;
        }

        public static MeshGeometry3D CreateCube(Vector3D scale, bool freeze = true)
        {
            var mesh = new MeshGeometry3D
            {
                Positions = new Point3DCollection
                {
                    new Point3D(0, 0, 0),
                    new Point3D(scale.X, 0, 0),
                    new Point3D(scale.X, scale.Y, 0),
                    new Point3D(0, scale.Y, 0),
                    new Point3D(0, 0, -scale.Z),
                    new Point3D(scale.X, 0, -scale.Z),
                    new Point3D(scale.X, scale.Y, -scale.Z),
                    new Point3D(0, scale.Y, -scale.Z)
                },
                TriangleIndices = new Int32Collection
                {
                    // Front side
                    0, 1, 2,
                    0, 2, 3,
                    // Back side
                    4, 6, 5,
                    4, 7, 6,
                    // Top side
                    2, 6, 3,
                    3, 6, 7,
                    // Bottom side
                    0, 4, 1,
                    1, 4, 5,
                    // Left side
                    3, 7, 0,
                    0, 7, 4,
                    // Right side
                    1, 5, 2,
                    2, 5, 6,
                }
            };

            if (freeze && mesh.CanFreeze)
            {
                mesh.Freeze();
            }

            return mesh;
        }
    }
}
