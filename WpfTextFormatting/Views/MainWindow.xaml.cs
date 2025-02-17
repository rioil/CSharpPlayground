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

            //var fontRendering = new FontRendering(20, TextAlignment.Left, [], Brushes.Black, new Typeface("Arial"));
            //var textSource = new CustomTextSource(pixelsPerDip, fontRendering);
            var fontRenderings = new FontRendering[] {
                new(26, TextAlignment.Left, [], Brushes.Red, new Typeface("Arial")),
                new(20, TextAlignment.Left, [], Brushes.Blue, new Typeface("Arial")),
                new(30, TextAlignment.Left, [], Brushes.Green, new Typeface("Arial")),
                new(28, TextAlignment.Left, [], Brushes.Orange, new Typeface("Arial")),
            };
            var textSource = new ColorfulTextSource(pixelsPerDip, fontRenderings);

            var textSourcePosition = 0;
            var linePosition = new Point(0, 0);

            textDest = new DrawingGroup();
            using var drawingContext = textDest.Open();

            textSource.Text = "Hello, World!";

            var formatter = TextFormatter.Create();
            while (textSourcePosition < textSource.Text.Length)
            {
                //var paragraphProperties = new GenericTextParagraphProperties(textSource.FontRendering, pixelsPerDip);
                var paragraphProperties = new GenericTextParagraphProperties(textSource.FontRenderings[0], pixelsPerDip);
                using var textLine = formatter.FormatLine(textSource, textSourcePosition, 96 * 6, paragraphProperties, null);
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
