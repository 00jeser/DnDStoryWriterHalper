using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorPicker;
using DnDStoryWriterHalper.Services;
using Microsoft.Win32;

namespace DnDStoryWriterHalper.Components
{
    public partial class TextEditor : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextEditor),
                new PropertyMetadata("", TextPropertyChangedCallback));

        private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextEditor te)
                te._richTextBox.Text = e.NewValue as string;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public TextEditor()
        {
            InitializeComponent();
        }

        private void ColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(e.NewValue ?? Colors.Black));
        }

        private void AddHyperLink(object sender, RoutedEventArgs e)
        {
            var dialog = new LinkSettingDialogWindow();
            dialog.ShowDialog();
            (var text, var link) = dialog.result;

            if (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(link))
                return;
            var h = new Hyperlink();
            var r = new Run("[" + text + "]");
            r.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 233, 215));
            h.Inlines.Add(r);
            h.NavigateUri = new Uri("dnd://" + link.Replace(' ', '_'));

            var p = new Paragraph(h);
            p.KeepTogether = true;
            _richTextBox.Document.Blocks.Add(p);
        }

        private void _richTextBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var il = _richTextBox.Selection.Start.Paragraph.Inlines.FirstInline;
                if (il is Hyperlink h)
                {
                    var a = h.NavigateUri;
                    NavigationEventsProvider.Instance.NavigateTo(a.Host, this);
                }
            }
            catch (Exception)
            {

            }
        }

        private void _richTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void OnColorChanged(object sender, RoutedEventArgs e)
        {
        }
        private void _richTextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox rtb)
                if (rtb?.Selection.GetPropertyValue(TextElement.ForegroundProperty) is SolidColorBrush scBrush)
                    ColorPicker.SelectedColor = scBrush.Color;
        }

        private void ColorPicker_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty,
                new SolidColorBrush((sender as PortableColorPicker)?.SelectedColor ?? Colors.Black));
        }

        private void _richTextBox_OnFormatedTextChanged(object? sender, EventArgs e)
        {
            Text = _richTextBox.FormatedText;
        }
    }
}
