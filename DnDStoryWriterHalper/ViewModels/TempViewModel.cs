using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;

namespace DnDStoryWriterHalper.ViewModels
{
    public class TempViewModel : ViewModelBase
    {
        private ObservableCollection<object> _objects;

        public ObservableCollection<object> Objects
        {
            get => _objects;
            set
            {
                _objects = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IDirrectoryComponent> _newO = new ObservableCollection<IDirrectoryComponent>()
        {
            new Dirrectory()
            {
                Name = "1", Content = new ObservableCollection<object>()
                {
                    new TextPage() {Name = "2"}
                }
            }
        };
        public ObservableCollection<IDirrectoryComponent> newObjects
        {
            get => _newO;
        }
        

        public TempViewModel()
        {
            Objects = ProjectService.Instance.Components;
        }
    }
}
