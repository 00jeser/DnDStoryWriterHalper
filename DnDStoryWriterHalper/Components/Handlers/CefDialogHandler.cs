using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;

namespace DnDStoryWriterHalper.Components.Handlers
{
    public class CefDialogHandler : IDialogHandler
    {
        public bool OnFileDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFileDialogMode mode, string title,
            string defaultFilePath, List<string> acceptFilters, IFileDialogCallback callback)
        {
            string? file = null;
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    var fd = new FileSelectDialogWindow();
                    fd.ShowDialog();
                    file = fd.Result?.Name;
                }
            );
            if (file == null)
                return false;
            callback.Continue(new List<string>() { ProjectService.Instance.UnzipFileByName(file) });
            return true;
        }
    }
}
