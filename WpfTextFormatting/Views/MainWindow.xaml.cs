using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace WpfTextFormatting.Views
{
    /* 
     * If some events were receive from ViewModel, then please use PropertyChangedWeakEventListener and CollectionChangedWeakEventListener.
     * If you want to subscribe custome events, then you can use LivetWeakEventListener.
     * When window closing and any timing, Dispose method of LivetCompositeDisposable is useful to release subscribing events.
     *
     * Those events are managed using WeakEventListener, so it is not occurred memory leak, but you should release explicitly.
     */
    public partial class MainWindow : Window
    {
        private CustomTextSource? _textSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
            UpdateFormattedText(newDpi.PixelsPerDip);
        }

        private void UpdateFormattedText(double pixelsPerDip)
        {
            if (!IsLoaded) { return; }

            var fontRendering = new FontRendering(20, TextAlignment.Left, [], Brushes.Black, new Typeface("Arial"));
            _textSource = new CustomTextSource(pixelsPerDip, fontRendering);

            var textSourcePosition = 0;
            var linePosition = new Point(0, 0);

            textDest = new DrawingGroup();
            using var drawingContext = textDest.Open();

            _textSource.Text = "Hello, World!";

            var formatter = TextFormatter.Create();
            while (textSourcePosition < _textSource.Text.Length)
            {
                var paragraphProperties = new GenericTextParagraphProperties(_textSource.FontRendering, pixelsPerDip);
                using var textLine = formatter.FormatLine(_textSource, textSourcePosition, 96 * 6, paragraphProperties, null);
                textLine.Draw(drawingContext, linePosition, InvertAxes.None);
                linePosition.Y += textLine.Height;
                textSourcePosition += textLine.Length;
            }

            textDrawingBrush.Drawing = textDest;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateFormattedText(VisualTreeHelper.GetDpi(this).PixelsPerDip);
        }
    }
}
