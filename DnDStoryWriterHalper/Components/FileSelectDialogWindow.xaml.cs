using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;
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

namespace DnDStoryWriterHalper.Components
{
    /// <summary>
    /// Логика взаимодействия для FileSelectDialogWindow.xaml
    /// </summary>
    public partial class FileSelectDialogWindow : Window
    {
        public FileItem? Result;
        public FileSelectDialogWindow()
        {
            InitializeComponent();
            filesCB.ItemsSource = ProjectService.Instance.GetAllFiles();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = filesCB.SelectedItem as FileItem;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            this.Close();
        }
    }
}
