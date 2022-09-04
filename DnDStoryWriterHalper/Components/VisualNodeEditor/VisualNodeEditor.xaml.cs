using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using DnDStoryWriterHalper.Extensions;
using DnDStoryWriterHalper.Services;
using HandyControl.Data;

namespace DnDStoryWriterHalper.Components.VisualNodeEditor
{
    /// <summary>
    /// Логика взаимодействия для VisualNodeEditor.xaml
    /// </summary>
    public partial class VisualNodeEditor : UserControl
    {
        #region StrokesData
        public static DependencyProperty StrokesDataProperty = DependencyProperty.Register(
            nameof(StrokesData),
            typeof(string),
            typeof(VisualNodeEditor),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                StrokesDataChanged
                )
            );

        private static void StrokesDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d as VisualNodeEditor).NeedToUpdateWhenDataChenged)
            {
                (d as VisualNodeEditor).NeedToUpdateWhenDataChenged = true;
                return;
            }

            if (e.NewValue == "")
            {
                (d as VisualNodeEditor).ink.Strokes = new StrokeCollection();
                return;
            }
            using var stream = new MemoryStream();
            stream.Write(Convert.FromBase64String(e.NewValue.ToString()));
            stream.Seek(0, SeekOrigin.Begin);
            (d as VisualNodeEditor).ink.Strokes = new StrokeCollection(stream);
        }

        public string StrokesData
        {
            get => (string)GetValue(StrokesDataProperty);
            set => SetValue(StrokesDataProperty, value);
        }
        #endregion

        #region FiguresData

        public static readonly DependencyProperty FigureDataProperty = DependencyProperty.Register(
            "FigureData",
            typeof(string),
            typeof(VisualNodeEditor),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                FigureDataPropertyChangedCallback
                )
            );

        private static void FigureDataPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d as VisualNodeEditor).NeedToUpdateWhenDataChenged)
            {
                (d as VisualNodeEditor).NeedToUpdateWhenDataChenged = true;
                return;
            }
            var vne = d as VisualNodeEditor;
            vne.ink.Children.Clear();
            if (e.NewValue == null || e.NewValue is not string)
            {
                return;
            }
            string[] figures = e.NewValue.ToString().Split('|');
            foreach (var figure in figures)
            {
                string[] figureData = figure.Split(';'); // type;color;width;height;x;y;data
                if (figureData.Length != 7)
                    continue;
                FrameworkElement c = new FrameworkElement();
                switch (figureData[0])
                {
                    case "0":
                        c = new Rectangle() { Fill = new SolidColorBrush(Convert.ToInt64(figureData[1], 16).ToARGBColor()), Tag = figureData.Last() };
                        break;
                    case "1":
                        c = new Ellipse() { Fill = new SolidColorBrush(Convert.ToInt64(figureData[1], 16).ToARGBColor()), Tag = figureData.Last() };
                        break;
                    case "2":
                        c = new EditableTextBlock { MultiLine = true, FontSize = 20 };
                        (c as EditableTextBlock).Text = figureData[6];
                        break;
                }
                c.Width = double.Parse(figureData[2]);
                c.Height = double.Parse(figureData[3]);
                c.PreviewMouseDown += (sender, args) =>
                {
                    NavigationEventsProvider.Instance.NavigateTo((sender as Shape).Tag.ToString());
                };
                InkCanvas.SetLeft(c, double.Parse(figureData[4]));
                InkCanvas.SetTop(c, double.Parse(figureData[5]));
                vne.ink.Children.Add(c);
            }
        }

        public string FigureData
        {
            get { return (string)GetValue(FigureDataProperty); }
            set { SetValue(FigureDataProperty, value); }
        }
        #endregion

        public bool NeedToUpdateWhenDataChenged = true;

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
                var a = new EditableTextBlock() { Width = 100, Height = 100, Text = "SomeText", MultiLine = true, FontSize = 20 };
                ink.Children.Add(a);
                InkCanvas.SetTop(a, e.GetPosition(ink).Y - 50);
                InkCanvas.SetLeft(a, e.GetPosition(ink).X - 50);
            }
            UpdateFiguresData();
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
        private bool _isScrolling;
        private void Ink_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Middle)
                _isScrolling = true;
            _lastPoint = e.GetPosition(IncScrollViewer);
        }

        private void Ink_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isScrolling)
            {
                IncScrollViewer.ScrollToHorizontalOffset(IncScrollViewer.HorizontalOffset + (_lastPoint.X - e.GetPosition(IncScrollViewer).X));
                IncScrollViewer.ScrollToVerticalOffset(IncScrollViewer.VerticalOffset + (_lastPoint.Y - e.GetPosition(IncScrollViewer).Y));
            }
            _lastPoint = e.GetPosition(IncScrollViewer);
        }

        private void Ink_OnMouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.ButtonState == MouseButtonState.Released && e.ChangedButton == MouseButton.Middle)
                _isScrolling = false;
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

        private void UpdateStrokesData()
        {
            NeedToUpdateWhenDataChenged = false;
            string strokes = "";
            var a = ink.Children;
            using var s = new MemoryStream();
            ink.Strokes.Save(s);
            s.Position = 0;
            strokes = Convert.ToBase64String(s.ToArray());

            StrokesData = strokes;
        }

        private void UpdateFiguresData()
        {
            NeedToUpdateWhenDataChenged = false;
            StringBuilder figures = new();

            foreach (UIElement child in ink.Children)
            {   // type;color;width;height;x;y;data
                figures.Append('|');
                figures.Append(child switch
                {
                    Rectangle => '0',
                    Ellipse => '1',
                    EditableTextBlock => '2',
                    _ => '0'
                });
                figures.Append(';');
                figures.Append(child switch
                {
                    Shape s => (s.Fill as SolidColorBrush)?.Color.ToLong().ToString("X"),
                    _ => ""
                });
                figures.Append(';');
                figures.Append((child as FrameworkElement)?.Width.ToString("F1"));
                figures.Append(';');
                figures.Append((child as FrameworkElement)?.Height.ToString("F1"));
                figures.Append(';');
                figures.Append(InkCanvas.GetLeft(child).ToString("F0"));
                figures.Append(';');
                figures.Append(InkCanvas.GetTop(child).ToString("F0"));
                figures.Append(';');
                figures.Append(child switch
                {
                    EditableTextBlock etb => etb.Text,
                    Rectangle r => r.Tag?.ToString() ?? "",
                    Ellipse e => e.Tag?.ToString() ?? "",
                    _ => ""
                });
            }

            if (figures[0] == '|')
                figures.Remove(0, 1);

            FigureData = figures.ToString();
        }


        private async void Ink_OnInitialized(object? sender, EventArgs e)
        {
            IncScrollViewer.ScrollToVerticalOffset(5000);
            IncScrollViewer.ScrollToHorizontalOffset(5000);
        }

        private void Ink_OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            UpdateStrokesData();
        }

        private void Ink_OnStrokeErased(object sender, RoutedEventArgs e)
        {
            UpdateStrokesData();
        }

        private void Ink_OnSelectionMoved(object? sender, EventArgs e)
        {
            UpdateStrokesData();
            UpdateFiguresData();
        }

        private void Ink_OnSelectionResized(object? sender, EventArgs e)
        {
            UpdateStrokesData();
            UpdateFiguresData();
        }

        private void Ink_OnSelectionChanged(object? sender, EventArgs e)
        {
            if (ink.GetSelectedElements().Count == 1 && ink.GetSelectedElements()[0] is Shape)
            {
                AddLinkBtn.IsEnabled = true;
            }
            else
            {
                AddLinkBtn.IsEnabled = false;
            }
        }

        private void AddLinkBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectPageDialogWindow();
            dialog.ShowDialog();
            if (dialog.Result != null)
            {
                (ink.GetSelectedElements()[0] as Shape).Tag = dialog.Result.Guid;
                UpdateFiguresData();
                (ink.GetSelectedElements()[0] as Shape).PreviewMouseDown += (sender, args) =>
                {
                    NavigationEventsProvider.Instance.NavigateTo((sender as Shape).Tag.ToString());
                };
            }
        }
    }
}
