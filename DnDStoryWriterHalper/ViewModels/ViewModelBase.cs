using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DnDStoryWriterHalper.Annotations;

namespace DnDStoryWriterHalper.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void ChangeProperty<T>(ref T propery, T newVal,
            [CallerMemberName] string? propertyName = null)
        {
            propery = newVal;
            OnPropertyChanged(propertyName);
        }
    }
}
