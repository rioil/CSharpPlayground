using System;
using System.Windows.Media.TextFormatting;

namespace WpfTextFormatting.Views
{
    internal class CustomTextSource : TextSource
    {
        public CustomTextSource(double pixelsPerDip, FontRendering fontRendering)
        {
            PixelsPerDip = pixelsPerDip;
            FontRendering = fontRendering;
        }

        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// フォントレンダリング
        /// </summary>
        public FontRendering FontRendering { get; set; }

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

            if (textSourceCharacterIndex < Text.Length)
            {
                return new TextCharacters(
                    Text,
                    textSourceCharacterIndex,
                    Text.Length - textSourceCharacterIndex,
                    new GenericTextRunProperties(FontRendering, PixelsPerDip));
            }

            return new TextEndOfParagraph(1);
        }
    }
}
