using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Avalonia.Controls;
using StoryWriterHalper.Models;
using StoryWriterHalper.Services;

namespace StoryWriterHalper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ProjectService.Instance.LoadFromFile("C:\\Users\\00jes\\OneDrive\\Desktop\\Story\\1.zip");
            
        }
    }
}