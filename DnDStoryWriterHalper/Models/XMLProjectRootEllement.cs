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
        [XmlArrayItem("dirrectory", typeof(Dirrectory))]
        [XmlArrayItem("imagepage", typeof(ImagePage))]
        [XmlArrayItem("textpage", typeof(TextPage))]
        [XmlArrayItem("browserpage", typeof(BrowserPage))]
        public ObservableCollection<object> Items { get; set; }
    }
}
