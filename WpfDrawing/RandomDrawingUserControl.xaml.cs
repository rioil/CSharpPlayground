using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfDrawing
{
    /// <summary>
    /// RandomDrawingUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class RandomDrawingUserControl : UserControl
    {
        private readonly DispatcherTimer _timer = new();

        public RandomDrawingUserControl()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InitializeImage();
        }

        private void InitializeImage()
        {
            Image.Source = CreateDrawingImage();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                Image.Source = CreateDrawingImage();
            };
            _timer.Start();
        }

        private static DrawingImage CreateDrawingImage()
        {
            var drawingImage = new DrawingImage();
            var group = new DrawingGroup();
            for (int i = 0; i < 10; i++)
            {
                group.Children.Add(CreateRandomDrawing());
            }
            drawingImage.Drawing = group;

            if (drawingImage.CanFreeze)
            {
                drawingImage.Freeze();
            }
            return drawingImage;
        }

        private static GeometryDrawing CreateRandomDrawing()
        {
            var drawing = new GeometryDrawing
            {
                Brush = DrawingUtil.GetRandomBrush(),
                Pen = new Pen(Brushes.Black, 2),
                Geometry = GetRandomGeometry()
            };

            if (drawing.CanFreeze)
            {
                drawing.Freeze();
            }
            return drawing;
        }

        private static Geometry GetRandomGeometry()
        {
            var value = Random.Shared.Next(0, 4);
            return value switch
            {
                0 => CreateEllipse(),
                1 => CreateRectangle(),
                2 => CreateLine(),
                _ => CreatePath(),
            };

            static EllipseGeometry CreateEllipse()
            {
                var r = Random.Shared.Next(5, 15);
                return new EllipseGeometry(DrawingUtil.GetRandomPosition(), r, r);
            }

            static RectangleGeometry CreateRectangle()
            {
                return new RectangleGeometry(new Rect(DrawingUtil.GetRandomPosition(), DrawingUtil.GetRandomSize()));
            }

            static LineGeometry CreateLine()
            {
                return new LineGeometry(DrawingUtil.GetRandomPosition(), DrawingUtil.GetRandomPosition());
            }

            static PathGeometry CreatePath()
            {
                return new PathGeometry([
                    new PathFigure(
                        DrawingUtil.GetRandomPosition(),
                        [ new ArcSegment(DrawingUtil.GetRandomPosition(), DrawingUtil.GetRandomSize(), 0, false, SweepDirection.Counterclockwise, true)],
                        false
                    )
                ]);
            }
        }
    }
}
