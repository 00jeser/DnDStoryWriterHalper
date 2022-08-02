using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using StoryWriterHalper.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace StoryWriterHalper.Views
{
    public partial class PageViewer : UserControl
    {
        public static readonly StyledProperty<object> ItemProperty =
            AvaloniaProperty.Register<PageViewer, object>(nameof(Item));

        public object Item
        {
            get { return GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        public PageViewer()
        {
            InitializeComponent();
            ItemProperty.Changed.Subscribe(ItemChanged);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private static void ItemChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is ContentControl contentControl)
                contentControl.Content = e.NewValue;
        }

        private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            Item = e.AddedItems.IndexOf(0);
        }
    }
}
