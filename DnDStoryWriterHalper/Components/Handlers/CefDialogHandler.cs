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
            var file = (ProjectService.Instance.Components.First(x => x is FilesContainer) as FilesContainer).Files.Select(x => x.Name).First();
            callback.Continue(new List<string>(){ ProjectService.Instance.UnzipFileByName(file) });
            return true;
        }
    }
}
