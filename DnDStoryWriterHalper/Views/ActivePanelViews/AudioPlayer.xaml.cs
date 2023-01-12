using HandyControl.Controls;
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

namespace DnDStoryWriterHalper.Views.ActivePanelViews
{
    /// <summary>
    /// Логика взаимодействия для AudioPlayer.xaml
    /// </summary>
    public partial class AudioPlayer : Border
    {
        public AudioPlayer()
        {
            InitializeComponent();
            
        }

        private void CheckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lst = new List<string>();
            foreach(var item in (sender as CheckComboBox).SelectedItems) 
            {
                lst.Add(item.ToString());
            }
            vm.SearchGenres = new List<string>(lst);
        }
    }
}
