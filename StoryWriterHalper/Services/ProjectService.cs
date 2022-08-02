using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StoryWriterHalper.Models;
using StoryWriterHalper.ViewModels;

namespace StoryWriterHalper.Services
{
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
            var ls = new ObservableCollection<object>(Components.ToArray());
            Task t = new Task(() =>
            {
                using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        if (entry.Name == "project.xml")
                        {
                            lock (ls)
                            {
                                new XmlSerializer(typeof(XMLProjectRootEllement)).Serialize(entry.Open(),
                                    new XMLProjectRootEllement()
                                    {
                                        Items = ls
                                    });
                            }
                        }
                    }
                }
            });
            t.Start();

            CurrentFile = file;
        }

        public async void SaveData()
        {
            var ls = new ObservableCollection<object>(Components.ToArray());
            Task t = new Task(() =>
            {
                using (ZipArchive zip = ZipFile.Open(CurrentFile, ZipArchiveMode.Update))
                {

                    try
                    {
                        foreach (var e in zip.Entries)
                        {
                            if (e.Name == "project.xml")
                            {
                                using (var es = e.Open())
                                using (var sw = new StreamWriter(es))
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
        }

        public void AddFile(Stream file, string name)
        {

        }

        public void RemoveFile(string name)
        {

        }


        private static ProjectService? instance;

        public static ProjectService Instance
        {
            get => instance ??= new ProjectService();
            set => instance = value;
        }

    }
}
