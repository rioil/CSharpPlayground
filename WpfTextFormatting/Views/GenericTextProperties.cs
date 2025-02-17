using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace WpfTextFormatting.Views
{
    internal class GenericTextParagraphProperties : TextParagraphProperties
    {
        public GenericTextParagraphProperties(
            TextRunProperties defaultTextRunProperties,
            bool firstLineInParagraph,
            FlowDirection flowDirection,
            double indent,
            double lineHeight,
            TextAlignment textAlignment,
            TextMarkerProperties textMarkerProperties,
            TextWrapping textWrapping)
        {
            DefaultTextRunProperties = defaultTextRunProperties;
            FirstLineInParagraph = firstLineInParagraph;
            FlowDirection = flowDirection;
            Indent = indent;
            LineHeight = lineHeight;
            TextAlignment = textAlignment;
            TextMarkerProperties = textMarkerProperties;
            TextWrapping = textWrapping;
        }

        public GenericTextParagraphProperties(FontRendering fontRendering, double pixelsPerDip)
        {
            DefaultTextRunProperties = new GenericTextRunProperties(fontRendering, pixelsPerDip);
            FirstLineInParagraph = false;
            FlowDirection = FlowDirection.LeftToRight;
            Indent = 0;
            LineHeight = fontRendering.FontSize;
            TextAlignment = fontRendering.Alignment;
            TextMarkerProperties = null;
            TextWrapping = TextWrapping.NoWrap;
        }

        public override TextRunProperties DefaultTextRunProperties { get; }

        public override bool FirstLineInParagraph { get; }

        public override FlowDirection FlowDirection { get; }

        public override double Indent { get; }

        public override double LineHeight { get; }

        public override TextAlignment TextAlignment { get; }

        public override TextMarkerProperties? TextMarkerProperties { get; }

        public override TextWrapping TextWrapping { get; }
    }

    internal class GenericTextRunProperties : TextRunProperties
    {
        public GenericTextRunProperties(
            Brush backgroundBrush,
            CultureInfo cultureInfo,
            double fontHintingEmSize,
            double fontRenderingEmSize,
            Brush foregroundBrush,
            TextDecorationCollection textDecorations,
            TextEffectCollection textEffects,
            Typeface typeface)
        {
            BackgroundBrush = backgroundBrush;
            CultureInfo = cultureInfo;
            FontHintingEmSize = fontHintingEmSize;
            FontRenderingEmSize = fontRenderingEmSize;
            ForegroundBrush = foregroundBrush;
            TextDecorations = textDecorations;
            TextEffects = textEffects;
            Typeface = typeface;
        }

        public GenericTextRunProperties(FontRendering fontRendering, double pixelsPerDip)
        {
            BackgroundBrush = null;
            CultureInfo = CultureInfo.CurrentUICulture;
            FontHintingEmSize = fontRendering.FontSize;
            FontRenderingEmSize = fontRendering.FontSize;
            ForegroundBrush = fontRendering.TextColor;
            TextDecorations = fontRendering.Decorations;
            TextEffects = null;
            Typeface = fontRendering.Typeface;
        }

        public override Brush? BackgroundBrush { get; }

        public override CultureInfo CultureInfo { get; }

        public override double FontHintingEmSize { get; }

        public override double FontRenderingEmSize { get; }

        public override Brush ForegroundBrush { get; }

        public override TextDecorationCollection? TextDecorations { get; }

        public override TextEffectCollection? TextEffects { get; }

        public override Typeface Typeface { get; }
    }
}
