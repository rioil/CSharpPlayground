using System.Windows.Media.Media3D;

namespace Wpf3D.Controls
{
    internal static class Model3DFactory
    {
        public static GeometryModel3D Create(Geometry3D geometry, Material material, Transform3D transform, bool freeze = true)
        {
            var model = new GeometryModel3D
            {
                Geometry = geometry,
                Material = material,
                Transform = transform,
            };

            if (freeze && model.CanFreeze)
            {
                model.Freeze();
            }

            return model;
        }

        public static GeometryModel3D CreateXZPlane(double xLength, double zLength, Material material, bool freeze = true)
        {
            var mesh = Mesh3DFactory.CreateXZPlane(xLength, zLength);
            return Create(mesh, material, Transform3D.Identity, freeze);
        }

        public static GeometryModel3D CreateCube(Vector3D position, Vector3D scale, Material material, bool freeze = true)
        {
            var mesh = Mesh3DFactory.CreateCube(scale);
            var transform = new TranslateTransform3D(position);
            if (freeze && transform.CanFreeze)
            {
                transform.Freeze();
            }
            return Create(mesh, material, transform, freeze);
        }
    }
}
