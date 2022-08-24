using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DnDStoryWriterHalper.Models
{
    public class FileItem : IDirrectoryComponent
    {
        [XmlElement("name")]
        public string? Name { get; set; }
        [XmlElement("filename")]
        public string? FileName { get; set; }

        public override string ToString() => Name ?? FileName ?? "No name file";
    }
}
