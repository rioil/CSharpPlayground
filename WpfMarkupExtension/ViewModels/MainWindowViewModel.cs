using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WpfMarkupExtension.Models;

namespace WpfMarkupExtension.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
        }

        public string MyProperty
        {
            get => _myProperty;
            set => RaisePropertyChangedIfSet(ref _myProperty, value);
        }
        private string _myProperty = string.Empty;
    }
}
