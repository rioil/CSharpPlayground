using Livet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ContentControlTemplateSelector.Models
{
    public abstract class ItemBase : NotificationObject
    {
        /// <summary>
        /// 値
        /// </summary>
        public string Value
        {
            get => _value;
            set => RaisePropertyChangedIfSet(ref _value, value);
        }
        private string _value;

        public ItemBase(string value)
        {
            _value = value;
        }
    }

    public abstract class ItemGroupBase : ItemBase, IEnumerable<ItemBase>
    {
        public ObservableCollection<ItemBase> ChildItems
        {
            get => _childItems;
            set => RaisePropertyChangedIfSet(ref _childItems, value);
        }
        private ObservableCollection<ItemBase> _childItems = new();

        public ItemGroupBase(string value) : base(value) { }

        public void Add(ItemBase item)
        {
            ChildItems.Add(item);
        }

        public IEnumerator<ItemBase> GetEnumerator()
        {
            return ChildItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ChildItems.GetEnumerator();
        }
    }

    public class VerticalItemGroup : ItemGroupBase {
        public VerticalItemGroup(string value) : base(value) { }
    }

    public class HorizontalItemGroup : ItemGroupBase {
        public HorizontalItemGroup(string value) : base(value) { }
    }

    public class RedItem : ItemBase {
        public RedItem(string value) : base(value) { }
    }

    public class GreenItem : ItemBase {
        public GreenItem(string value) : base(value) { }
    }
}
