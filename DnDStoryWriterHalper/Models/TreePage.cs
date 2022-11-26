using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.Models
{
    public class TreePage : IPage
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Data { get; set; }

        public TreePage()
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = "Дерево";
            Data = ";()";
        }
    }
}
