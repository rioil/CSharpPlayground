using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
    ///     <MyNamespace:RandomDrawingControl/>
    ///
    /// </summary>
    public class RandomDrawingControl : FrameworkElement
    {
        private const int NumOfLines = 1_000;
        private readonly Line[] _lines = new Line[NumOfLines];
        private readonly Dictionary<SolidColorBrush, Pen> _penCache = [];
        private readonly DrawingGroup _lineDrawings = new();

        private readonly Pen _crossLinePen;
        private readonly SolidColorBrush _crossLineBrush;

        private readonly Pen _selectRectPen;

        private SelectionRect? _selectionRect;

        private Transform _displayTransform = Transform.Identity;

        private Point _lastMousePosition;

        static RandomDrawingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RandomDrawingControl), new FrameworkPropertyMetadata(typeof(RandomDrawingControl)));
        }

        public RandomDrawingControl()
        {
            _crossLineBrush = DrawingUtil.TransparentRedBrush;
            _crossLinePen = new Pen(_crossLineBrush, 3)
            {
                DashStyle = new DashStyle([1, 1], 0),
                DashCap = PenLineCap.Flat
            };
            _crossLinePen.TryFreeze();

            _selectRectPen = new Pen(Brushes.White, 1);
            _selectRectPen.TryFreeze();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InitializeLines();
        }

        private void InitializeLines()
        {
            using var context = _lineDrawings.Open();
            for (int i = 0; i < NumOfLines; i++)
            {
                var startPoint = DrawingUtil.GetRandomPosition();
                var endPoint = DrawingUtil.GetRandomPosition();
                var lineBrush = DrawingUtil.GetRandomBrush();
                if (!_penCache.TryGetValue(lineBrush, out var pen))
                {
                    pen = new Pen(lineBrush, 10);
                    pen.TryFreeze();
                    _penCache.Add(lineBrush, pen);
                }
                context.DrawLine(pen, startPoint, endPoint);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // draw background
            drawingContext.DrawRectangle(Brushes.Black, null, new Rect(0, 0, ActualWidth, ActualHeight));

            // draw lines
            _lineDrawings.Transform = _displayTransform;
            drawingContext.DrawDrawing(_lineDrawings);

            // draw cross lines
            if (IsMouseOver)
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

            // draw selection rectangle
            if (_selectionRect.HasValue)
            {
                drawingContext.DrawRectangle(Brushes.Transparent, _selectRectPen, _selectionRect.Value);
            }

            // draw mouse coordinate
            if (IsMouseOver)
            {
                var position = _displayTransform.Inverse.Transform(Mouse.GetPosition(this));
                var text = new FormattedText($"{position.X:F2}, {position.Y:F2}", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.White, 1);
                drawingContext.DrawText(text, new Point(5, ActualHeight - 15));
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                _selectionRect = default;
            }
            else if (Mouse.LeftButton == MouseButtonState.Pressed && !_selectionRect.HasValue)
            {
                var pos = Mouse.GetPosition(this);
                _selectionRect = new SelectionRect(pos);
            }
            InvalidateVisual();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var position = e.GetPosition(this);

            if (_selectionRect.HasValue)
            {
                _selectionRect = _selectionRect.Value with { EndPoint = position };
            }

            if (e.MouseDevice.MiddleButton == MouseButtonState.Pressed)
            {
                var scrollDeltaMatrix = new Matrix(1, 0, 0, 1, position.X - _lastMousePosition.X, position.Y - _lastMousePosition.Y);
                _displayTransform = new MatrixTransform(_displayTransform.Value * scrollDeltaMatrix);
            }

            _lastMousePosition = position;
            InvalidateVisual();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!IsMouseOver) { return; }
            var pos = Mouse.GetPosition(this);
            _selectionRect = new SelectionRect(pos);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _selectionRect = default;
            InvalidateVisual();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                var zoomDelta = e.Delta / 120d / 10d;
                if (_displayTransform.Value.M11 + zoomDelta < 0.1d)
                {
                    return;
                }

                var center = e.MouseDevice.GetPosition(this);
                var zoomDeltaMatrix = new Matrix(1 + zoomDelta, 0, 0, 1 + zoomDelta, -center.X * zoomDelta, -center.Y * zoomDelta);
                _displayTransform = new MatrixTransform(_displayTransform.Value * zoomDeltaMatrix);
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                var scrollDelta = e.Delta / 120d * 20;
                var scrollDeltaMatrix = new Matrix(1, 0, 0, 1, scrollDelta, 0);
                _displayTransform = new MatrixTransform(_displayTransform.Value * scrollDeltaMatrix);
            }
            else
            {
                var scrollDelta = e.Delta / 120d * 20;
                var scrollDeltaMatrix = new Matrix(1, 0, 0, 1, 0, scrollDelta);
                _displayTransform = new MatrixTransform(_displayTransform.Value * scrollDeltaMatrix);
            }
            InvalidateVisual();
        }

        internal record struct Line(LineGeometry Geometry, SolidColorBrush Brush, Pen Pen);
        internal struct SelectionRect(Point startPoint, Point endPoint)
        {
            public SelectionRect(Point startPoint) : this(startPoint, startPoint) { }

            public Point StartPoint { get; set; } = startPoint;
            public Point EndPoint { get; set; } = endPoint;

            public static implicit operator Rect(SelectionRect selectionRect) => new(selectionRect.StartPoint, selectionRect.EndPoint);
        }
    }
}
