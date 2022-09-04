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
using System.Windows.Shapes;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;

namespace DnDStoryWriterHalper.Components
{
    /// <summary>
    /// Логика взаимодействия для SelectPageDialogWindow.xaml
    /// </summary>
    public partial class SelectPageDialogWindow : Window
    {
        public IPage? Result = null;
        public SelectPageDialogWindow()
        {
            InitializeComponent();
            pagesCB.ItemsSource = ProjectService.Instance.GetAllPages();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = (IPage)pagesCB.SelectedItem;
            this.Close();
        }
    }
}
