using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DnDStoryWriterHalper.ViewModels;

namespace DnDStoryWriterHalper.Models
{
    public class TextPage : IPage
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

        [XmlElement("id")]
        public string Guid { get; set; }

        public TextPage(string name, string text)
        {
            Name = name;
            Text = text;
            Guid = System.Guid.NewGuid().ToString();
        }

        public TextPage(string name) : this(name,
            "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2{\\fonttbl{\\f0\\fcharset0 Times New Roman;}{\\f2\\fcharset0 Segoe UI;}}{\\colortbl\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0{\\lang1033\\fs38\\f2\\cf0 \\cf0\\ql{\\f2 {\\cf1\\ltrch }\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}\r\n}\r\n}")
        {

        }

        public TextPage() : this("Новая текстовая страница")
        {

        }

    }
}
