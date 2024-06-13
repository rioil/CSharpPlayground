using System.Windows;

namespace WpfDrawing
{
    internal static class FreezableExtensions
    {
        /// <summary>
        /// Freeze if <see cref="Freezable.CanFreeze"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="freezable"></param>
        /// <returns></returns>
        public static bool TryFreeze(this Freezable freezable)
        {
            if (freezable.IsFrozen)
            {
                return true;
            }

            if (freezable.CanFreeze)
            {
                freezable.Freeze();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
