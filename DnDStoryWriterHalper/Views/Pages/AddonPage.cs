using DnDStoryWriterHalper.Components;
using DnDStoryWriterHalper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DnDStoryWriterHalper.Views.Pages;

public class AddonPageDataTemplateCreator
{
    //https://stackoverflow.com/questions/248362/how-do-i-build-a-datatemplate-in-c-sharp-code
    public DataTemplate GetNewAddonDataTemplate()
    {
        DataTemplate cardLayout = new DataTemplate();
        cardLayout.DataType = typeof(AddonPage);
        FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(AddonViewer));
        spFactory.Name = "myComboFactory";
        spFactory.SetBinding(AddonViewer.PageModelProperty, new Binding() { Mode = BindingMode.OneWay });
        cardLayout.VisualTree = spFactory;
        return cardLayout;
    }
}
