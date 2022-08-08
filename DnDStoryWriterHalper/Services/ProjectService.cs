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
            if (!string.IsNullOrWhiteSpace(ofd.FileName))
            {
                CurrentFile = ofd.FileName;
            }
        }
        var ls = new ObservableCollection<object>(Components.ToArray());
        Task t = new Task(() =>
        {
            using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
            {

                try
                {
                    var e = zip.GetEntry("project.xml");
                    if (e != null && e.Name != null)
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

    public bool AddFileAsItem(string filePath, string fileName)
    {
        if (string.IsNullOrWhiteSpace(CurrentFile))
            return false;
        bool success = false;
        var zippedfilename = "";
        using (var file = new FileStream(filePath, FileMode.Open))
        {
            using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
            {
                zippedfilename = Guid.NewGuid() + "." + fileName.Split('.').Last();
                using (var e = zip.CreateEntry(zippedfilename).Open())
                {
                    file.CopyTo(e);
                    success = true;
                }
            }
        }
        File.Delete(filePath);

        if (success)
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    var fcAsObject = Components.FirstOrDefault(x => x is FilesContainer, null);
                    FilesContainer FileContainer;
                    if (fcAsObject == null)
                    {
                        FileContainer = new FilesContainer();
                        Components.Insert(0, FileContainer);
                    }
                    else
                    {
                        FileContainer = fcAsObject as FilesContainer;
                    }
                    FileContainer.Files.Add(new FileItem()
                    {
                        FileName = zippedfilename,
                        Name = fileName
                    });
                });
        }
        return success;
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
                Components.Insert(Components.FirstOrDefault(x => true, null) is FilesContainer ? 1 : 0, page);
            else
                Components.Add(page);
        }
    }

    public string UnzipFileByName(string fileName)
    {
        FilesContainer fc = Components.FirstOrDefault(x => x is FilesContainer, null) as FilesContainer;
        foreach (var f in fc.Files)
        {
            if (f.Name == fileName)
            {
                return UnzipFileByPath(f.FileName);
            }
        }

        return "";
    }
    public string UnzipFileByPath(string fileName)
    {
        var dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tempfiles");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, fileName);
        using (var file = new FileStream(path, FileMode.OpenOrCreate))
        {
            using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
            {
                var en = zip.GetEntry(fileName);
                if (en == null)
                    return "";
                using (var e = en.Open())
                {
                    e.CopyTo(file);
                }
            }
        }

        return path;
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


    public List<IPage> GetAllPages(ObservableCollection<object> list = null)
    {
        var rez = new List<IPage>();
        foreach (var component in list ?? Components)
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

    public void ClearTempFiles()
    {
        var dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tempfiles");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        foreach (var file in Directory.GetFiles(dir))
        {
            File.Delete(file);
        }
    }

    private static ProjectService? instance;

    public static ProjectService Instance
    {
        get => instance ??= new ProjectService();
        set => instance = value;
    }

}

