using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;
using Microsoft.Win32;

namespace DnDStoryWriterHalper.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<object> _items = new();

        public ObservableCollection<object> Items
        {
            get => _items;
            set => ChangeProperty(ref _items, value);
        }

        private IDirrectoryComponent _selectedItem;

        public IDirrectoryComponent SelectedItem
        {
            get => _selectedItem;
            set => ChangeProperty(ref _selectedItem, value);
        }

        private ICommand _loadCommand = new Command((p) =>
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "*.zip|*.zip";
            fd.ShowDialog();
            if(string.IsNullOrEmpty(fd.FileName) || !File.Exists(fd.FileName))
                return;
            ProjectService.Instance.LoadFromFile(fd.FileName);
        });

        public ICommand LoadCommand
        {
            get => _loadCommand;
            set => ChangeProperty(ref _loadCommand, value);
        }

        private ICommand _newCommand;

        public ICommand NewCommand
        {
            get => _newCommand;
            set => ChangeProperty(ref _newCommand, value);
        }

        private ICommand _saveCommand = new Command((p) =>
        {
            ProjectService.Instance.SaveData();
        });

        public ICommand SaveCommand
        {
            get => _saveCommand;
            set => ChangeProperty(ref _saveCommand, value);
        }

        public MainViewModel()
        {
            Items = ProjectService.Instance.Components;
            _newCommand = new Command(p =>
            {
                ProjectService.Instance.NewProject();
                SelectedItem = null;
            });
        }
    }
}
