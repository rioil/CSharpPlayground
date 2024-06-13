using System.Windows;
using System.Windows.Media;

namespace WpfDrawing
{
    internal static class DrawingUtil
    {
        private const double BrushOpacity = 0.5;
        public static SolidColorBrush TransparentRedBrush { get; } = CreateFreezedBrush(Colors.Red, BrushOpacity);
        public static SolidColorBrush TransparentGreenBrush { get; } = CreateFreezedBrush(Colors.Green, BrushOpacity);
        public static SolidColorBrush TransparentBlueBrush { get; } = CreateFreezedBrush(Colors.Blue, BrushOpacity);
        public static SolidColorBrush TransparentYellowBrush { get; } = CreateFreezedBrush(Colors.Yellow, BrushOpacity);

        public static SolidColorBrush GetRandomBrush()
        {
            var value = Random.Shared.Next(0, 4);
            return value switch
            {
                0 => TransparentRedBrush,
                1 => TransparentGreenBrush,
                2 => TransparentBlueBrush,
                _ => TransparentYellowBrush,
            };
        }

        public static Point GetRandomPosition()
        {
            const int Min = 0;
            const int Max = 400;
            return new Point(Random.Shared.Next(Min, Max), Random.Shared.Next(Min, Max));
        }

        public static Size GetRandomSize()
        {
            return new Size(Random.Shared.Next(10, 100), Random.Shared.Next(10, 100));
        }

        private static SolidColorBrush CreateFreezedBrush(Color color, double opacity)
        {
            var brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();

            return brush;
        }
    }
}