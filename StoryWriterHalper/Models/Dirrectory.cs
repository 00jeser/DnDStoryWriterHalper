using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StoryWriterHalper.Models
{

    public class Dirrectory : IDirrectory
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlArray("content")]
        [XmlArrayItem("dirrectory", typeof(Dirrectory))]
        [XmlArrayItem("textpage", typeof(TextPage))]
        public ObservableCollection<object> Content { get; set; }
    }
}
