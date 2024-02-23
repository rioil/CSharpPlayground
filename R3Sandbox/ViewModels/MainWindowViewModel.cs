using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using R3;
using R3Playground.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace R3Playground.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            IsAliveDebounced = IsAlive.Debounce(TimeSpan.FromSeconds(3), TimeProvider.System).ToReadOnlyReactiveProperty();

            // TODO うまく動いてない Debounceが思ってる動作と違うかも？TimeProviderが上手く動いてない可能性も無いわけではない
            // ValueTimeoutDetectionの同等物を実装したつもり
            DeadTime = Observable.Merge(
                IsAlive.Select<bool, DateTime?>(x => DateTime.Now).Debounce(TimeSpan.FromSeconds(3), TimeProvider.System),
                IsAlive.Select<bool, DateTime?>(x => null)
            ).ToReadOnlyReactiveProperty(null);

            IsAlive.Subscribe(x => Debug.WriteLine($"IsAlive Changed: {x}"));
        }

        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
        }

        public ReactiveProperty<bool> IsAlive { get; } = new(false);
        public ReadOnlyReactiveProperty<bool> IsAliveDebounced { get; }

        public ReadOnlyReactiveProperty<DateTime?> DeadTime { get; }
    }
}
