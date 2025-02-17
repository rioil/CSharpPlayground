using System;
using System.Windows.Media.TextFormatting;

namespace WpfTextFormatting.Views
{
    internal class ColorfulTextSource : TextSource
    {
        public string Text { get; set; } = string.Empty;

        public FontRendering[] FontRenderings { get; }

        public ColorfulTextSource(double pixelsPerDip, FontRendering[] fontRenderings)
        {
            PixelsPerDip = pixelsPerDip;
            FontRenderings = fontRenderings;
        }

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
        {
            var cbr = new CharacterBufferRange(Text, 0, textSourceCharacterIndexLimit);
            return new TextSpan<CultureSpecificCharacterBufferRange>(
                textSourceCharacterIndexLimit,
                new CultureSpecificCharacterBufferRange(System.Globalization.CultureInfo.CurrentCulture, cbr));
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new NotImplementedException();
        }

        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(textSourceCharacterIndex, 0);

            if (textSourceCharacterIndex >= Text.Length)
            {
                return new TextEndOfParagraph(1);
            }

            var fontRendering = FontRenderings[textSourceCharacterIndex % FontRenderings.Length];
            return new TextCharacters(
                Text,
                textSourceCharacterIndex,
                1,
                new GenericTextRunProperties(fontRendering, PixelsPerDip));
        }
    }
}
