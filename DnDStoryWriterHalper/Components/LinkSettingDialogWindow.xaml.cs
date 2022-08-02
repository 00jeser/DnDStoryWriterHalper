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
using Wpf.Ui.Controls;

namespace DnDStoryWriterHalper.Components
{
    /// <summary>
    /// Логика взаимодействия для LinkSettingDialogWindow.xaml
    /// </summary>
    public partial class LinkSettingDialogWindow : Window
    {
        public (string, string) result;
        public LinkSettingDialogWindow()
        {
            InitializeComponent();
            LinkTB.ItemsSource = ProjectService.Instance.GetAllPages();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            result = (TextTB.Text, (LinkTB.SelectedItem as IPage)?.Guid??"null");
            this.Close();
        }

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            result = ("", "");
            this.Close();
        }
    }
}
