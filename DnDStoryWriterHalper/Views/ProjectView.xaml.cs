﻿using System;
using System.Collections.Generic;
using System.IO;
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
using DnDStoryWriterHalper.Components;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;
using Microsoft.Win32;

namespace DnDStoryWriterHalper.Views
{
    /// <summary>
    /// Логика взаимодействия для ProjectView.xaml
    /// </summary>
    public partial class ProjectView : Grid
    {
        public ProjectView()
        {
            InitializeComponent();
            NavigationEventsProvider.Instance.LinkClicked += (sender, args) =>
            {
                for (int i = 0; i < tv.ItemContainerGenerator.Items.Count; i++)
                {
                    var tvi = (tv.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem);
                    if (tvi != null && tv.ItemContainerGenerator.ItemFromContainer(tvi) is IPage page)
                    {
                        if (args.Url==page.Guid)
                        {
                            tvi.IsSelected = true;
                        }
                    }
                }
            };
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var tvi = tv.ItemContainerGenerator.ContainerFromItem(tv.SelectedItem)
                as TreeViewItem;
            tvi.IsSelected = false;
        }

        private void AddTextPage(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                ProjectService.Instance.AddPageOrDirrecory(new TextPage(), mi.CommandParameter);
            }
        }

        private void RemovePageOrDirrectory(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
                ProjectService.Instance.RemovePageOrDirrecory(mi.CommandParameter);
        }

        private void AddDir(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                ProjectService.Instance.AddPageOrDirrecory(new Dirrectory(), mi.CommandParameter);
            }
        }

        private void AddImagePage(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProjectService.Instance.CurrentFile))
            {
                MessageBox.Show("Сохраните проект перед добавлением файлов");
                return;
            }
            if (sender is MenuItem mi)
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.ShowDialog();
                if (File.Exists(fd.FileName))
                {
                    string fileName = Guid.NewGuid()+".png";
                    using (var st = new FileStream(fd.FileName, FileMode.Open))
                    {
                        ProjectService.Instance.AddFile(st, fileName);
                    }

                    ProjectService.Instance.AddPageOrDirrecory(new ImagePage()
                    {
                        Name = "Новое изображение",
                        FileName = fileName
                    },mi.CommandParameter);
                }
            }
        }

        private void AddBrowserPage(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                UrlEnterWindow w = new UrlEnterWindow();
                w.ShowDialog();
                ProjectService.Instance.AddPageOrDirrecory(new BrowserPage("Сcылка",w.Result), mi.CommandParameter);
            }
        }
    }
}
