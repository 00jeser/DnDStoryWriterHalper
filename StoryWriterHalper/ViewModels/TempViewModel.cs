using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using StoryWriterHalper.Models;
using StoryWriterHalper.Services;

namespace StoryWriterHalper.ViewModels
{
    public class TempViewModel : ReactiveObject
    {
        private ObservableCollection<object> _data = new();

        public ReactiveCommand<Unit, Unit> Save { get; }

        public ObservableCollection<object> Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        private object _selectedPage;

        public object SelectedPage
        {
            get => _selectedPage;
            set => this.RaiseAndSetIfChanged(ref _selectedPage, value);
        }

        public TempViewModel()
        {
            Data = ProjectService.Instance.Components;
            Save = ReactiveCommand.Create(() =>
            {
                ProjectService.Instance.SaveData();
            });
        }
    }
}
