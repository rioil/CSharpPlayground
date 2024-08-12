using Livet;
using R3;
using System;
using System.Diagnostics;
using System.Linq;

namespace R3Playground.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            IsAliveDebounced = IsAlive.Debounce(TimeSpan.FromSeconds(1), TimeProvider.System).ToReadOnlyBindableReactiveProperty();

            // TODO うまく動いてない Debounceが思ってる動作と違うかも？TimeProviderが上手く動いてない可能性も無いわけではない
            // ValueTimeoutDetectionの同等物を実装したつもり
            DeadTime = Observable.Merge(
                IsAlive.Where(x => x).Select<bool, DateTime?>(x => DateTime.Now).Debounce(TimeSpan.FromSeconds(3), TimeProvider.System),
                IsAlive.Select<bool, DateTime?>(x => null)
            ).ToReadOnlyBindableReactiveProperty(null);

            IsAlive.Subscribe(x => Debug.WriteLine($"IsAlive Changed: {x}"));
            IsAliveDebounced.AsObservable().Subscribe(x => Debug.WriteLine($"IsAliveDebounced Changed: {x}"));
        }

        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
        }

        public ReactiveProperty<bool> IsAlive { get; } = new(false);
        public IReadOnlyBindableReactiveProperty<bool> IsAliveDebounced { get; }

        public IReadOnlyBindableReactiveProperty<DateTime?> DeadTime { get; }
    }
}
