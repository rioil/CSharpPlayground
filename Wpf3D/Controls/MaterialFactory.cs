using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Wpf3D.Controls
{
    internal static class MaterialFactory
    {
        public static Material CreateDiffuseMaterial(Color color, double opacity = 1.0, bool freeze = true)
        {
            var material = new DiffuseMaterial
            {
                Brush = new SolidColorBrush(color) { Opacity = opacity }
            };

            if (freeze && material.CanFreeze)
            {
                material.Freeze();
            }

            return material;
        }
    }
}
