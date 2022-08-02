using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StoryWriterHalper.Models
{
    public record class TextPage : IPage
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlIgnore]
        public string Text { get; set; }

        [XmlElement("text")]
        public string XMLText
        {
            get =>
                Text.Replace("\n", "&#xA;")
                    .Replace("\r", "&#xD;")
                    .Replace("\t", "&#x9;");
            set =>
                Text = value.Replace("&#xA;", "\n")
                    .Replace("&#xD;", "\r")
                    .Replace("&#x9;", "\t");
        }
    }
}
