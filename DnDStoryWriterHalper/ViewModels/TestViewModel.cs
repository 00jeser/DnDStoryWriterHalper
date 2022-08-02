using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        private string _text = "13456789";

        public string Text
        {
            get => _text;
            set => ChangeProperty(ref _text, value);
        }


    }
}
