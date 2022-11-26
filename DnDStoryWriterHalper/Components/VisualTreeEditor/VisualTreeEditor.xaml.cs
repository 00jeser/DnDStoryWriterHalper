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

namespace DnDStoryWriterHalper.Components.VisualTreeEditor
{
    /// <summary>
    /// Логика взаимодействия для VisualTreeEditor.xaml
    /// </summary>
    public partial class VisualTreeEditor : UserControl
    {
        public static readonly DependencyProperty TreeProperty = DependencyProperty.Register(
            nameof(Tree),
            typeof(TreeModel),
            typeof(VisualTreeEditor),
            //new FrameworkPropertyMetadata(
            //        new TreeModel(),
            //        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
            //    )
            new PropertyMetadata(new TreeModel(null), TreeChanged)
            );

        private static void TreeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VisualTreeEditor).DataContext = e.NewValue;
        }

        public TreeModel Tree
        {
            get => (TreeModel)GetValue(TreeProperty);
            set
            {
                SetValue(TreeProperty, value);
                value.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(TreeModel.Data))
                    {
                        Data = (o as TreeModel).Data;
                    }
                };
            }
        }



        public string Data
        {
            get { return (string)GetValue(DataProperty); }
            set
            {
                TreeModel newModel = new TreeModel(null);
                newModel.Data = value;
                Tree = newModel;
                SetValue(DataProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(string), typeof(VisualTreeEditor), new PropertyMetadata("", DataPropertyCanged));

        private static void DataPropertyCanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VisualTreeEditor).Data = e.NewValue as string;
        }

        public VisualTreeEditor()
        {
            InitializeComponent();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Tree.Children.Add(new TreeModel(Tree) { Name = "Name" });
        }
    }
}
