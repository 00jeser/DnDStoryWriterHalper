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
    /// Логика взаимодействия для FileSaveDialogWindow.xaml
    /// </summary>
    public partial class FileSaveDialogWindow : Window
    {
        public string? Result;
        public FileSaveDialogWindow(string fileName)
        {
            InitializeComponent();
            filesTB.Text = fileName;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = string.IsNullOrWhiteSpace(filesTB.Text) ? null : filesTB.Text;
            this.Close();
        }
    }
}
