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
    /// Логика взаимодействия для UrlEnterWindow.xaml
    /// </summary>
    public partial class UrlEnterWindow : Window
    {
        public string Result = "";
        public UrlEnterWindow()
        {
            InitializeComponent();
        }

        private void Cancle_Clic(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = "";
            if (!UrlBox.Text.StartsWith("https://") && !UrlBox.Text.StartsWith("http://"))
            {
                Result += "https://";
            }
            Result += UrlBox.Text;

            this.Close();
        }

        private void UrlBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (UrlBox.Text.StartsWith("https://") || UrlBox.Text.StartsWith("http://"))
            {
                try
                {
                    new Uri(UrlBox.Text);
                    okBtn.IsEnabled = true;
                }
                catch (Exception)
                {
                    okBtn.IsEnabled = false;
                }
                httpsLable.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    new Uri("https://" + UrlBox.Text);
                    okBtn.IsEnabled = true;
                }
                catch (Exception)
                {
                    okBtn.IsEnabled = false;
                }
                httpsLable.Visibility = Visibility.Visible;
            }
        }
    }
}
