using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using DnDStoryWriterHalper.Components.Helpers.FontAwesome;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;
using Microsoft.Win32;
using Path = System.IO.Path;

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
            NavigationEventsProvider.Instance.LinkClicked += GoTo;

            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Addons");
            foreach (var addon in Directory.GetDirectories(path))
            {
                var metadataFilePath = Path.Combine(addon, "metadata.json");
                Dictionary<string, string>? metadata;
                if (File.Exists(metadataFilePath))
                {
                    metadata = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(metadataFilePath));
                }
                else
                {
                    continue;
                }
                var addonName = addon.Split('\\').Last();
                var l = new Label() { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 0), FontSize = 19 };
                ImageAwesome.SetFontAwesome(l, Symbols.squarePlus);
                MenuItem addonItem = new MenuItem()
                {
                    Header = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            l,
                            new TextBlock() {Text = metadata?["name"]??addonName}
                        }
                    }
                };
                var l2 = new Label() { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 0), FontSize = 19 };
                ImageAwesome.SetFontAwesome(l2, Symbols.squarePlus);
                MenuItem addonItem2 = new MenuItem()
                {
                    Header = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            l2,
                            new TextBlock() {Text = metadata?["name"]??addonName}
                        }
                    }
                };
                foreach (var addonPage in Directory.GetDirectories(addon))
                {
                    Dictionary<string, string>? pageMetadata;
                    var pageMetadataPath = Path.Combine(addonPage, "metadata.json");
                    if (File.Exists(pageMetadataPath))
                    {
                        pageMetadata = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(pageMetadataPath));
                    }
                    else
                    {
                        continue;
                    }
                    var pageName = addonPage.Split('\\').Last();
                    if (Directory.GetFiles(addonPage).Any(x => x.EndsWith("index.html")))
                    {
                        var item = new MenuItem()
                        {
                            Header = pageMetadata?["name"] ?? addonName,
                        };
                        item.Click += (o, e) => AddAddonPage(addonName, pageName, null, pageMetadata?["default name"]);
                        addonItem.Items.Add(item);
                        var item2 = new MenuItem()
                        {
                            Header = pageMetadata?["name"] ?? addonName,
                        };
                        item2.SetBinding(MenuItem.CommandParameterProperty, new Binding());
                        item2.Click += (o, e) => AddAddonPage(addonName, pageName, (o as MenuItem).CommandParameter);
                        addonItem2.Items.Add(item2);
                    }
                }
                ((Resources["TreeContextMenu"] as ContextMenu)?.Items[0] as MenuItem)?.Items.Add(addonItem);
                ((Resources["DirrectoryContextMenu"] as ContextMenu)?.Items[0] as MenuItem)?.Items.Add(addonItem2);
            }
        }

        private void GoTo(object? sender, NavigationEventsProvider.LinkEventArgs e)
        {
            for (int i = 0; i < tv.ItemContainerGenerator.Items.Count; i++)
            {
                var tvi = (tv.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem);
                if (tvi != null && tv.ItemContainerGenerator.ItemFromContainer(tvi) is IPage page)
                {
                    if (e.Url == page.Guid)
                    {
                        tvi.IsSelected = true;
                    }
                }
                if (tvi != null && tv.ItemContainerGenerator.ItemFromContainer(tvi) is IDirrectory dir)
                {
                    GoToRecursion(e.Url, tvi);
                }
            }
        }
        private void GoToRecursion(string Url, TreeViewItem item)
        {
            for (int i = 0; i < item.ItemContainerGenerator.Items.Count; i++)
            {
                var tvi = (item.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem);
                if (tvi != null && item.ItemContainerGenerator.ItemFromContainer(tvi) is IPage page)
                {
                    if (Url == page.Guid)
                    {
                        tvi.IsSelected = true;
                    }
                }
                if (tvi != null && item.ItemContainerGenerator.ItemFromContainer(tvi) is IDirrectory dir)
                {
                    GoToRecursion(Url, tvi);
                }
            }
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
                    string fileName = Guid.NewGuid() + ".png";
                    using (var st = new FileStream(fd.FileName, FileMode.Open))
                    {
                        ProjectService.Instance.AddFile(st, fileName);
                    }

                    ProjectService.Instance.AddPageOrDirrecory(new ImagePage()
                    {
                        Name = "Новое изображение",
                        FileName = fileName
                    }, mi.CommandParameter);
                }
            }
        }

        private void AddBrowserPage(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                UrlEnterWindow w = new UrlEnterWindow();
                w.ShowDialog();
                ProjectService.Instance.AddPageOrDirrecory(new BrowserPage("Сcылка", w.Result), mi.CommandParameter);
            }
        }

        private void AddAddonPage(string addon, string page, object dir, string? pageName = null)
        {
            ProjectService.Instance.AddPageOrDirrecory(new AddonPage { PluginName = addon, PageName = page, Name = pageName ?? "AddonPage", Content = "" }, dir);
        }

        private void AddCanvasPage(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                ProjectService.Instance.AddPageOrDirrecory(new CanvasPage(), mi.CommandParameter);
            }
        }

        private void AddTreePage(object sender, RoutedEventArgs e)
        {

            if (sender is MenuItem mi)
            {
                ProjectService.Instance.AddPageOrDirrecory(new TreePage(), mi.CommandParameter);
            }
        }
    }
}
