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
        [XmlArrayItem("canvaspage", typeof(CanvasPage))]
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public ObservableCollection<object> Items { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    }
}
