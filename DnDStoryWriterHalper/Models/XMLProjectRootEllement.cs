using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DnDStoryWriterHalper.Models
{
    [XmlRoot("project")]
    public record class XMLProjectRootEllement
    {
        [XmlArray("items")]
        [XmlArrayItem("file", typeof(FileItem))]
        [XmlArrayItem("dirrectory", typeof(Dirrectory))]
        [XmlArrayItem("imagepage", typeof(ImagePage))]
        [XmlArrayItem("textpage", typeof(TextPage))]
        [XmlArrayItem("browserpage", typeof(BrowserPage))]
        [XmlArrayItem("filescontainer", typeof(FilesContainer))]
        [XmlArrayItem("addonpage", typeof(AddonPage))]
        public ObservableCollection<object> Items { get; set; }
    }
}
