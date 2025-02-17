using System.Windows;
using System.Windows.Media;

namespace WpfTextFormatting.Views
{
    internal record FontRendering(
        double FontSize,
        TextAlignment Alignment,
        TextDecorationCollection Decorations,
        Brush TextColor,
        Typeface Typeface)
    {
        public FontRendering() : this(12, TextAlignment.Left, [], Brushes.Black, new Typeface("Arial")) { }
    }
}
