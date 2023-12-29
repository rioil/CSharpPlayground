using System;
using System.Windows;
using System.Windows.Markup;

namespace WpfMarkupExtension.MarkupExtensions
{
    // ref. https://learn.microsoft.com/en-us/dotnet/api/system.windows.markup.markupextension.providevalue?view=windowsdesktop-8.0
    internal class PropertyBackingFieldExtension : MarkupExtension
    {
        private readonly string _propertyPath;

        public PropertyBackingFieldExtension(string path)
        {
            _propertyPath = path;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueTarget = serviceProvider.GetService<IProvideValueTarget>();
            if (valueTarget is null) { return string.Empty; }

            if (valueTarget.TargetObject is not FrameworkElement frameworkElement || frameworkElement.DataContext is null)
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }

    internal static class IServiceProviderExtensions
    {
        public static T? GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
