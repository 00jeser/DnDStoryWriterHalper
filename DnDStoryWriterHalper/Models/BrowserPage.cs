using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.Models
{
    public class BrowserPage : IPage
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Url { get; set; }

        public BrowserPage(string Name, string Url)
        {
            this.Name = Name;
            this.Url = Url;
            Guid = System.Guid.NewGuid().ToString();
        }

        public BrowserPage():this("Ссылка", "https://github.com/OOjeser/DnDStoryWriterHalper"){}
    }
}
