using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static WpfDrawing.RandomDrawingControl;

namespace WpfDrawing
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfDrawing"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfDrawing;assembly=WpfDrawing"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:RandomDrawingCanvasControl/>
    ///
    /// </summary>
    public class RandomDrawingCanvasControl : FrameworkElement
    {
        private const int NumOfLines = 1_000;

        private readonly Canvas _backgroundCanvas = new();

        private readonly CrossLineControl _crossLineControl = new();

        private readonly Canvas _lineCanvas = new();

        private readonly Pen _selectRectPen;
        private SelectionRect? _selectionRect;

        static RandomDrawingCanvasControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RandomDrawingCanvasControl), new FrameworkPropertyMetadata(typeof(RandomDrawingCanvasControl)));
        }

        public RandomDrawingCanvasControl()
        {
            _selectRectPen = new Pen(Brushes.White, 1);
            _selectRectPen.TryFreeze();

            _backgroundCanvas.Background = Brushes.Black;
            AddLogicalChild(_backgroundCanvas);
            AddVisualChild(_backgroundCanvas);

            InitializeLines();
            AddLogicalChild(_lineCanvas);
            AddVisualChild(_lineCanvas);

            _crossLineControl.IsHitTestVisible = false;
            AddLogicalChild(_crossLineControl);
            AddVisualChild(_crossLineControl);
        }

        #region base
        protected override Visual GetVisualChild(int index)
        {
            return index switch
            {
                0 => _backgroundCanvas,
                1 => _lineCanvas,
                2 => _crossLineControl,
                _ => throw new ArgumentOutOfRangeException(nameof(index)),
            };
        }

        protected override int VisualChildrenCount => 3;

        protected override Size MeasureOverride(Size availableSize)
        {
            _backgroundCanvas.Measure(availableSize);
            _lineCanvas.Measure(availableSize);
            _crossLineControl.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _backgroundCanvas.Arrange(new Rect(finalSize));
            _lineCanvas.Arrange(new Rect(finalSize));
            _crossLineControl.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }
        #endregion

        #region rendering
        private void InitializeLines()
        {
            for (int i = 0; i < NumOfLines; i++)
            {
                var startPoint = DrawingUtil.GetRandomPosition();
                var endPoint = DrawingUtil.GetRandomPosition();
                var lineBrush = DrawingUtil.GetRandomBrush();
                var line = new Line
                {
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y,
                    Stroke = lineBrush,
                    StrokeThickness = 10
                };
                _lineCanvas.Children.Add(line);
            }
        }
        #endregion

        #region mouse event handlers
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _crossLineControl.InvalidateVisual();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            _crossLineControl.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _crossLineControl.Visibility = Visibility.Collapsed;
        }
        #endregion
    }

    internal class CrossLineControl : FrameworkElement
    {
        private readonly Pen _crossLinePen;

        public CrossLineControl()
        {
            _crossLinePen = new Pen(DrawingUtil.TransparentRedBrush, 3)
            {
                DashStyle = new DashStyle([1, 1], 0),
                DashCap = PenLineCap.Flat
            };
            _crossLinePen.TryFreeze();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawCrossLines(drawingContext);
        }

        private void DrawCrossLines(DrawingContext drawingContext)
        {
            var position = Mouse.GetPosition(this);
            var xStartPoint = new Point(0, position.Y);
            var xEndPoint = new Point(ActualWidth, position.Y);
            var yStartPoint = new Point(position.X, 0);
            var yEndPoint = new Point(position.X, ActualHeight);

            drawingContext.DrawLine(_crossLinePen, xStartPoint, xEndPoint);
            drawingContext.DrawLine(_crossLinePen, yStartPoint, yEndPoint);
            drawingContext.DrawEllipse(Brushes.Red, null, position, 4, 4);
        }
    }
}
