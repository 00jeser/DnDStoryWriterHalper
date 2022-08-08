using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DnDStoryWriterHalper.Models
{
    public class FilesContainer : IDirrectoryComponent
    {
        [XmlIgnore]
        public string Name => "Хранилище файлов";
        [XmlArray("files")]
        [XmlArrayItem("file", typeof(FileItem))]
        public ObservableCollection<FileItem> Files { get; set; }

        public FilesContainer()
        {
            Files = new ObservableCollection<FileItem>();
        }
    }
}
