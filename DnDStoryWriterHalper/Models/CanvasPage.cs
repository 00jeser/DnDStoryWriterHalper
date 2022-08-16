using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.Models
{
    public class CanvasPage : IPage
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public string StrokesData { get; set; }
        public string FiguresData { get; set; }

        public CanvasPage()
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = "Холст";
            StrokesData = "";
            FiguresData = "";
        }
    }
}
