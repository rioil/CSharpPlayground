using ContentControlTemplateSelector.Models;
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

namespace ContentControlTemplateSelector.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
            var group = new VerticalItemGroup("Root Group")
            {
                new RedItem("Red Item 1"),
                new GreenItem("Green Item 1"),
                new HorizontalItemGroup("Child Group")
                {
                    new RedItem("Red Item 2"),
                    new GreenItem("Green Item 2")
                }
            };
            Item = group;
        }

        public ItemBase Item
        {
            get => _item;
            set => RaisePropertyChangedIfSet(ref _item, value);
        }
        private ItemBase _item = default!;


        public void Switch()
        {
            switch (Item)
            {
                case VerticalItemGroup vg:
                    Item = new HorizontalItemGroup(vg.Value) { ChildItems = vg.ChildItems };
                    break;
                case HorizontalItemGroup hg:
                    Item = new VerticalItemGroup(hg.Value) { ChildItems = hg.ChildItems };
                    break;
            }
        }
        private ViewModelCommand? _SwitchCommand;
        public ViewModelCommand SwitchCommand => _SwitchCommand ??= new ViewModelCommand(Switch);
    }
}
