using Livet;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ValueTimeoutDetection.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            var timeout = TimeSpan.FromSeconds(3);
            CompositeDisposable.Add(MyProperty.Where(x => x).Subscribe(x => MyProperty.Value = false));
            MyPropertyTriggeredTime = MyProperty.Timestamp()
                                                .Select(x => x.Timestamp.LocalDateTime)
                                                .ToReadOnlyReactivePropertySlim();

            IsMyPropertyExpired = Observable.Merge(
                MyProperty.Where(x => x).Select(x => false),
                MyProperty.Where(x => x).Throttle(timeout).Select(x => true))
                .ToReadOnlyReactivePropertySlim(true);
            MyPropertyExpiredTime = IsMyPropertyExpired.Timestamp()
                                                       .Select(x => x.Timestamp.LocalDateTime)
                                                       .ToReadOnlyReactivePropertySlim();
        }

        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
        }

        public ReactiveProperty<bool> MyProperty { get; } = new();
        public ReadOnlyReactivePropertySlim<DateTime> MyPropertyTriggeredTime { get; }

        public ReadOnlyReactivePropertySlim<bool> IsMyPropertyExpired { get; }
        public ReadOnlyReactivePropertySlim<DateTime> MyPropertyExpiredTime { get; }
    }
}
