using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDStoryWriterHalper.Services
{
    public class NavigationEventsProvider
    {
        public class LinkEventArgs : EventArgs
        {
            public string Url { get; set; }
            public LinkEventArgs(string url) : base()
            {
                Url = url;
            }
        }
        private static NavigationEventsProvider? _instance;

        public static NavigationEventsProvider Instance
        {
            get => _instance ??= new NavigationEventsProvider();
            set => _instance = value;
        }


        public event EventHandler <LinkEventArgs> LinkClicked;

        public void NavigateTo(string guid, object? sender = null)
        {
            LinkClicked.Invoke(sender ?? this, new LinkEventArgs(guid));
        }
    }
}
