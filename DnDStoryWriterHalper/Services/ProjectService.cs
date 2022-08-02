using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.ViewModels;
using Microsoft.Win32;

namespace DnDStoryWriterHalper.Services;

public class ProjectService
{
    public string CurrentFile { get; set; }
    private ObservableCollection<object>? _components;
    public ObservableCollection<object>? Components
    {
        get => _components;
        set
        {
            if (value != null)
            {
                _components = value;
                _components.CollectionChanged += (o, e) =>
                    ComponentStateChanged?.Invoke(new ComponentsStateChangedEventArgs(Components));
            }
        }
    }

    public record class ComponentsStateChangedEventArgs(ObservableCollection<object> NewValue) { }
    public delegate void componentsStateChanged(ComponentsStateChangedEventArgs e);
    public event componentsStateChanged ComponentStateChanged;

    public ProjectService()
    {
        Components = new ObservableCollection<object>();
    }

    public void LoadFromFile(string file)
    {
        Components?.Clear();
        using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
        {
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.Name == "project.xml")
                {
                    XMLProjectRootEllement root = (XMLProjectRootEllement)new XmlSerializer(typeof(XMLProjectRootEllement)).Deserialize(entry.Open());
                    foreach (var i in root.Items)
                    {
                        Components.Add(i);
                    }
                }
            }
        }
        
        CurrentFile = file;
    }

    public async void SaveToFile(string file)
    {

    }

    public async Task SaveData()
    {
        if (string.IsNullOrWhiteSpace(CurrentFile))
        {
            var ofd = new SaveFileDialog();
            ofd.Filter = "*.zip|*.zip";
            ofd.ShowDialog();
            ofd.FileOk += (s, e) =>
            {
                CurrentFile = ofd.FileName;
            };
        }
        var ls = new ObservableCollection<object>(Components.ToArray());
        Task t = new Task(() =>
        {
            using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
            {

                try
                {
                    var e = zip.GetEntry("project.xml");
                    if (e.Name != null)
                    {
                        e.Delete();
                    }
                    var nn = zip.CreateEntry("project.xml");
                    using (var nns = nn.Open())
                    using (var sw = new StreamWriter(nns))
                    {
                        lock (ls)
                        {
                            new XmlSerializer(typeof(XMLProjectRootEllement)).Serialize(sw,
                                new XMLProjectRootEllement()
                                {
                                    Items = ls
                                });
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        });
        t.Start();
        await t.WaitAsync(new TimeSpan(0, 0, 10, 0, 0));
    }

    public void AddFile(Stream file, string name)
    {
        using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
        {
            using (var e = zip.CreateEntry(name).Open())
            {
                file.CopyTo(e);
            }
        }
    }

    public ImageSource GetImage(string name)
    {
        using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Read))
        {
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.Name == name)
                {
                    using (var es = entry.Open())
                    using (var memoryStream = new MemoryStream())
                    {
                        es.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = memoryStream;
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.EndInit();
                        return bi;
                    }
                }
            }
        }

        return null;
    }

    public void RemoveFile(string name)
    {

    }

    private bool delete(IDirrectory Node, object obj)
    {
        foreach (var o in Node.Content)
        {
            bool canBreak = false;
            if (o == obj)
                return Node.Content.Remove(o);
            if (o is IDirrectory dir)
            {
                canBreak = canBreak || delete(dir, obj);
            }

            if (canBreak)
                return true;
        }
        return false;
    }
    public bool RemovePageOrDirrecory(object page)
    {
        if (page is ImagePage ip)
        {
            using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
            {
                zip.GetEntry(ip.FileName)?.Delete();
            }
        }
        foreach (var o in Components)
        {
            if (o is IDirrectory dir)
                if (delete(dir, page))
                    return true;
            if (o == page)
                return Components.Remove(page);
        }

        return false;
    }
    public void AddPageOrDirrecory(object page, object dirrectory)
    {
        if (dirrectory is IDirrectory dir)
        {
            if (page is IDirrectory)
                dir.Content.Insert(0, page);
            else
                dir.Content.Add(page);
        }
        else
        {
            if (page is IDirrectory)
                Components.Insert(0, page);
            else
                Components.Add(page);
        }
    }

    public async void NewProject()
    {
        if (MessageBox.Show(Application.Current.MainWindow, "Сохранить?", "a?", MessageBoxButton.YesNo) ==
            MessageBoxResult.Yes)
        {
            await SaveData();
        }

        lock (Components)
        {
            Components.Clear();
        }
        CurrentFile = String.Empty;
    }


    public record class GuidNamePair(string Guid, string Name);
    public List<IPage> GetAllPages(ObservableCollection<object> list = null)
    {
        var rez = new List<IPage>();
        foreach (var component in list??Components)
        {
            switch (component)
            {
                case IPage p:
                    rez.Add(p);
                    break;
                case IDirrectory d:
                    rez.AddRange(GetAllPages(d.Content));
                    break;
            }
        }
        return rez;
    }

    private static ProjectService? instance;

    public static ProjectService Instance
    {
        get => instance ??= new ProjectService();
        set => instance = value;
    }

}

