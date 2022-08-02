using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DnDStoryWriterHalper.Models
{
    public class ImagePage : IPage
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("filename")]
        public string FileName { get; set; }
        [XmlElement("id")]
        public string Guid { get; set; }

        public ImagePage()
        {
            Name = "Изображение";
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}
