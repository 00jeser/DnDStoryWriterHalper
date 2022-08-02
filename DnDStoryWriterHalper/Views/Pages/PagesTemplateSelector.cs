using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DnDStoryWriterHalper.Models;

namespace DnDStoryWriterHalper.Views.Pages
{
    public class PagesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextPageDataTemplate { get; set; }
        public DataTemplate ImagePageDataTemplate { get; set; }
        public DataTemplate DefaultDataTemplate { get; set; }
        public DataTemplate DirrecotoryDataTemplate { get; set; }
        public DataTemplate BrowserPageDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case TextPage p:
                    return TextPageDataTemplate;
                case Dirrectory d:
                    return DirrecotoryDataTemplate;
                case ImagePage ip:
                    return ImagePageDataTemplate;
                case BrowserPage bp:
                    return BrowserPageDataTemplate;
                default:
                    return DefaultDataTemplate;
            }
        }
    }
}
