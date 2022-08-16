using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Data;

namespace DnDStoryWriterHalper.Components.VisualNodeEditor
{
    /// <summary>
    /// Логика взаимодействия для VisualNodeEditor.xaml
    /// </summary>
    public partial class VisualNodeEditor : UserControl
    {
        public static DependencyProperty StrokesDataProperty = DependencyProperty.Register(
            nameof(StrokesData), 
            typeof(byte[]), 
            typeof(VisualNodeEditor), 
            new FrameworkPropertyMetadata(
                Array.Empty<byte>(), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                StrokesDataChanged
                )
            );

        private static void StrokesDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is byte[] bb)
                using (var stream = new MemoryStream())
                {
                    stream.Write(bb);
                    stream.Seek(0, SeekOrigin.Begin);
                    (d as VisualNodeEditor).ink.Strokes = new StrokeCollection(stream);
                }
        }

        public byte[] StrokesData
        {
            get => (byte[])GetValue(StrokesDataProperty);
            set => SetValue(StrokesDataProperty, value);
        }

        public VisualNodeEditor()
        {
            InitializeComponent();
        }

        private void Rectangle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Ink_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Rectangle)))
            {
                var a = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(ColorPicker.SelectedColor) };
                ink.Children.Add(a);
                InkCanvas.SetTop(a, e.GetPosition(ink).Y - 50);
                InkCanvas.SetLeft(a, e.GetPosition(ink).X - 50);
            }
            if (e.Data.GetDataPresent(typeof(Ellipse)))
            {
                var a = new Ellipse() { Width = 100, Height = 100, Fill = new SolidColorBrush(ColorPicker.SelectedColor) };
                ink.Children.Add(a);
                InkCanvas.SetTop(a, e.GetPosition(ink).Y - 50);
                InkCanvas.SetLeft(a, e.GetPosition(ink).X - 50);
            }
            if (e.Data.GetDataPresent(typeof(Line)))
            {
                var a = new Line()
                {
                    Width = 100,
                    Height = 100,
                    Stroke = new SolidColorBrush(ColorPicker.SelectedColor),
                    StrokeThickness = StrokeWidth.Value,
                    X1 = 0,
                    X2 = 100,
                    Y1 = 0,
                    Y2 = 100
                };
                ink.Children.Add(a);
                InkCanvas.SetTop(a, e.GetPosition(ink).Y - 50);
                InkCanvas.SetLeft(a, e.GetPosition(ink).X - 50);
            }
            if (e.Data.GetDataPresent(typeof(TextBlock)))
            {
                var a = new EditableTextBlock() { Width = 100, Height = 100, Text = "SomeText" };
                ink.Children.Add(a);
                InkCanvas.SetTop(a, e.GetPosition(ink).Y - 50);
                InkCanvas.SetLeft(a, e.GetPosition(ink).X - 50);
            }
        }

        private void SetPen(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void SetSelect(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.Select;
        }

        private void SetErace(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void SetNone(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.None;
        }
        private void SetEraceAll(object sender, RoutedEventArgs e)
        {
            ink.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private Point _lastPoint;
        private bool _isMoved;
        private void Ink_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Middle)
                _isMoved = true;
            _lastPoint = e.GetPosition(IncScrollViewer);
        }

        private void Ink_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoved)
            {
                IncScrollViewer.ScrollToHorizontalOffset(IncScrollViewer.HorizontalOffset + (_lastPoint.X - e.GetPosition(IncScrollViewer).X));
                IncScrollViewer.ScrollToVerticalOffset(IncScrollViewer.VerticalOffset + (_lastPoint.Y - e.GetPosition(IncScrollViewer).Y));
            }
            _lastPoint = e.GetPosition(IncScrollViewer);
        }

        private void Ink_OnMouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.ButtonState == MouseButtonState.Released && e.ChangedButton == MouseButton.Middle)
                _isMoved = false;
        }

        private void IncScrollViewer_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Ink_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0 && IncViewbox.Height < 10000)
            {
                IncViewbox.Height += 100;
                IncViewbox.Width += 100;
            }

            if (e.Delta < 0 && IncViewbox.Height > 3000)
            {
                IncViewbox.Height -= 100;
                IncViewbox.Width -= 100;
            }
        }

        private void ColorPicker_OnColorChanged(object sender, RoutedEventArgs e)
        {
            ink.DefaultDrawingAttributes = new DrawingAttributes()
            {
                Color = ColorPicker.SelectedColor,
                Height = StrokeWidth.Value,
                Width = StrokeWidth.Value
            };
        }

        private void StrokeWidth_OnValueChanged(object? sender, FunctionEventArgs<double> e)
        {
            if (ink != null)
                ink.DefaultDrawingAttributes = new DrawingAttributes()
                {
                    Color = ColorPicker.SelectedColor,
                    Height = StrokeWidth.Value,
                    Width = StrokeWidth.Value,
                };
        }

        private void EllipseF_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, sender, DragDropEffects.Move);
        }

        private void RectangleF_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, sender, DragDropEffects.Move);
        }

        private void LineF_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, (sender as Grid).Children[0], DragDropEffects.Move);
        }

        private void TextBlockF_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, sender, DragDropEffects.Move);
        }

        private void Ink_OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            byte[] strokes;
            using (var s = new MemoryStream())
            {
                ink.Strokes.Save(s);
                s.Position = 0;
                strokes = s.ToArray();
            }
            
            StrokesData = strokes;
        }
    }
}
