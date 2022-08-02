﻿using System;
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
            var tb = (d as EditableTextBlock);
            tb.Block.Text = e.NewValue?.ToString();
            tb.Box.Text = e.NewValue?.ToString();
        }

        public string Text
        {
            get => GetValue(TextProperty).ToString();
            set => SetValue(TextProperty, value);
        }

        public EditableTextBlock()
        {
            InitializeComponent();
            ToggleToView();
        }

        public void ToggleToEdit()
        {
            Block.Visibility = Visibility.Hidden;
            Box.Visibility = Visibility.Visible;
        }

        public void ToggleToView()
        {
            Box.Visibility = Visibility.Hidden;
            Block.Visibility = Visibility.Visible;
        }

        private void UIElement_OnTextInput(object sender, TextCompositionEventArgs e)
        {
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
            Text = (sender as TextBox).Text;
        }
    }
}