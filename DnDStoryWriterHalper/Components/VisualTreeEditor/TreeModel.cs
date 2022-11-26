using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DnDStoryWriterHalper.Services;
using Microsoft.Win32;

namespace DnDStoryWriterHalper.Components.VisualTreeEditor
{
    public class TreeModel : ViewModels.ViewModelBase
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Data));
            }
        }

        private string _link;

        public string Link
        {
            get => _link;
            set
            {
                ChangeProperty(ref _link, value);
                OnPropertyChanged(nameof(Data));
            }
        }

        public ObservableCollection<TreeModel> Children { get; set; } = new();

        public Visibility IsHasNext => Children.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsHasPrevies => Parent != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsFirst => Parent?.Children.First() == this ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsLast => Parent?.Children.Last() == this ? Visibility.Collapsed : Visibility.Visible;

        private ICommand _delCommand;

        public ICommand DelCommand
        {
            get => _delCommand;
            set => ChangeProperty(ref _delCommand, value);
        }

        private ICommand _urlCommand;

        public ICommand UrlCommand
        {
            get { return _urlCommand; }
            set { _urlCommand = value; }
        }

        public string Data
        {
            get => GetData();
            set
            {

                Children.Clear();
                //u-r-l;name(u-r-l;name()\tu-r-l;name())
                var contentStart = value.IndexOf('(');
                var thisData = value.Substring(0, contentStart).Split(';');
                    Link = thisData[0];
                    Name = thisData[1];
                if(value.Length - 2 - contentStart != 0)
                {
                    var conetnt = value.Substring(contentStart + 1, value.Length-2 - contentStart);
                    List<string> words = new();
                    string word = "";
                    var brackets = 0;
                    foreach(var c in conetnt)
                    {
                        if (c == '(') brackets++;
                        if (c == ')') brackets--;

                        if (c == '\t' && brackets == 0) {
                            words.Add(word);
                            word = "";
                        }
                        else word += c;
                    }
                    if(word != "")
                        words.Add(word);
                    foreach(var c in words)
                    {
                        Children.Add(new TreeModel(this) { Data = c });
                    }
                }
                OnPropertyChanged(nameof(Data));
            }
        }



        public TreeModel? Parent { get; set; }

        public TreeModel(TreeModel? parent)
        {
            Parent = parent;
            _delCommand = new Command((p) =>
            {
                Parent?.Children.Remove(this);
            });
            _urlCommand = new Command((p) =>
            {
                if (string.IsNullOrWhiteSpace(_link))
                {
                    var dialogWindow = new SelectPageDialogWindow();
                    dialogWindow.ShowDialog();
                    _link = dialogWindow.Result?.Guid;
                }
                else
                {
                    NavigationEventsProvider.Instance.NavigateTo(_link);
                }
            });
            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateLinesVisible();
            foreach (var child in Children)
                child.UpdateLinesVisible();
            OnPropertyChanged(nameof(Data));
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var a = e.NewItems[0] as TreeModel;
                a.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Data));
            }

        }

        public void UpdateLinesVisible()
        {
            OnPropertyChanged(nameof(IsHasNext));
            OnPropertyChanged(nameof(IsHasPrevies));
            OnPropertyChanged(nameof(IsLast));
            OnPropertyChanged(nameof(IsFirst));
        }

        public string GetData()
        {
            StringBuilder rez = new StringBuilder();
            rez.Append(Link);
            rez.Append(';');
            rez.Append(Name);
            rez.Append('(');
            foreach (var tv in Children)
            {
                rez.Append(tv.Data);
                rez.Append('\t');
            }
            if (rez[rez.Length - 1] == '\t')
                rez.Remove(rez.Length - 1, 1);
            rez.Append(')');
            return rez.ToString();
        }
    }
}
