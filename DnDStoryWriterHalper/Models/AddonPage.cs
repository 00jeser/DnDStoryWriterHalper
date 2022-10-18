using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.Models
{
    public class AddonPage : IPage
    {
        public string Name { get; set; }
        public string Guid { get; set; }

        public string PluginName { get; init; }
        public string PageName { get; init; }
        public string Content { get; set; }

        public AddonPage():this("AddonPage", "", "", ""){}
        public AddonPage(string name, string pluginName, string pageName, string content)
        {
            Name = name;
            PluginName = pluginName;
            PageName = pageName;
            Content = content;
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}
