using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using DnDStoryWriterHalper.Services;

namespace DnDStoryWriterHalper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var a = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "";
            ProjectService.Instance.ClearTempFiles();
        }
    }
}
