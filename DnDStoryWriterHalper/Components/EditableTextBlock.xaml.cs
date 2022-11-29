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

namespace DnDStoryWriterHalper.Components
{
    /// <summary>
    /// Логика взаимодействия для EditableTextBlock.xaml
    /// </summary>
    public partial class EditableTextBlock : UserControl
    {
        public readonly static DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(EditableTextBlock),
                new PropertyMetadata("", PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock tb)
            {
                tb.Block.Text = e.NewValue?.ToString();
                tb.Box.Text = e.NewValue?.ToString();
            }
        }

        public string Text
        {
            get => GetValue(TextProperty).ToString();
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty MultiLineProperty = DependencyProperty.Register(
            "MultiLine", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(default(bool), MultiLinePropertyChangedCallback));

        private static void MultiLinePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var etb = d as EditableTextBlock;
            if (e.NewValue is bool b)
            {
                etb.Block.TextWrapping = TextWrapping.Wrap;
                etb.Box.TextWrapping = TextWrapping.Wrap;
                etb.Box.AcceptsReturn = true;
            }
        }

        public bool MultiLine
        {
            get { return (bool)GetValue(MultiLineProperty); }
            set { SetValue(MultiLineProperty, value); }
        }




        public string BlockedChars
        {
            get { return (string)GetValue(BlockedCharsProperty); }
            set { SetValue(BlockedCharsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlockedChars.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlockedCharsProperty =
            DependencyProperty.Register("BlockedChars", typeof(string), typeof(EditableTextBlock), new PropertyMetadata("", BlockedCharsChanged));

        private static void BlockedCharsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var etb = d as EditableTextBlock;
            if (e.NewValue is string s)
            {
                etb.BlockedChars = s;
            }
        }


        public enum ChangeModes
        {
            Always,
            WhenDone
        }


        public ChangeModes ChangeMode
        {
            get { return (ChangeModes)GetValue(ChangeModeProperty); }
            set { SetValue(ChangeModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChangeMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangeModeProperty =
            DependencyProperty.Register("ChangeMode", typeof(ChangeModes), typeof(EditableTextBlock), new PropertyMetadata(ChangeModes.Always, ChangeModeChanged));

        private static void ChangeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var etb = d as EditableTextBlock;
            if (e.NewValue is ChangeModes s)
            {
                etb.ChangeMode = s;
            }
        }



        //public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
        //    "FontSize", typeof(double), typeof(EditableTextBlock), new PropertyMetadata(default(double)));

        //public double FontSize
        //{
        //    get { return (double) GetValue(FontSizeProperty); }
        //    set { SetValue(FontSizeProperty, value); }
        //}

        public EditableTextBlock()
        {
            InitializeComponent();
            ToggleToView();
        }

        public async void ToggleToEdit()
        {
            Block.Visibility = Visibility.Hidden;
            Box.Visibility = Visibility.Visible;
            await Task.Delay(100);
            Box.Focus();
            Box.CaretIndex = Box.Text.Length;
        }

        public void ToggleToView()
        {
            Box.Visibility = Visibility.Hidden;
            Block.Visibility = Visibility.Visible;
            if(ChangeMode == ChangeModes.WhenDone) 
                Text = Box.Text;
        }

        private void UIElement_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ChangeMode == ChangeModes.Always)
                Text = (sender as TextBox).Text;
        }

        private void Box_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ToggleToView();
        }

        private void Box_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //ToggleToView();
        }

        private void Block_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Block.Focus();
                ToggleToEdit();
            }
        }

        private void Box_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Return)
            {
                ToggleToView();
            }
        }

        private void Box_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ChangeMode == ChangeModes.Always)
                Text = (sender as TextBox).Text;
        }

        private void Box_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (BlockedChars.Contains(e.Text) || e.Text == "\t")
            {
                e.Handled = true;
            }
        }

        private void Box_GotFocus(object sender, RoutedEventArgs e)
        {
            //ToggleToEdit();
        }
    }
}
