using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Hotwire.Resources
{
    public class AutoScrollingListview : ListView
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register(
                "AutoScroll",
                typeof(Boolean),
                typeof(AutoScrollingListview),
                new FrameworkPropertyMetadata(
                    true, //Default value.
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    AutoScroll_PropertyChanged));

        [Category("Common")]
        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        private static void AutoScroll_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { 
            SubscribeToAutoScroll_ItemsCollectionChanged((AutoScrollingListview)d, (bool)e.NewValue);
        }

        private static void SubscribeToAutoScroll_ItemsCollectionChanged(AutoScrollingListview listBox, bool subscribe)
        {
            INotifyCollectionChanged notifyCollection = listBox.Items.SourceCollection as INotifyCollectionChanged;
            if (notifyCollection != null)
                if (subscribe)
                    notifyCollection.CollectionChanged += listBox.AutoScroll_ItemsCollectionChanged;
                else
                    notifyCollection.CollectionChanged -= listBox.AutoScroll_ItemsCollectionChanged;
        }

        private void AutoScroll_ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int count = Items.Count;
                ScrollIntoView(Items[count - 1]);
            }
        }

        public AutoScrollingListview()
        {
            SubscribeToAutoScroll_ItemsCollectionChanged(this, (bool)AutoScrollProperty.DefaultMetadata.DefaultValue);
        }
    }
}
