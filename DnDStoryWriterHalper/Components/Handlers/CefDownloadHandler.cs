using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using DnDStoryWriterHalper.Services;

namespace DnDStoryWriterHalper.Components.Handlers
{
    public class CefDownloadHandler : IDownloadHandler
    {
        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return true;
        }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IBeforeDownloadCallback callback)
        {
            var dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tempfiles");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, "tmp.map");

            callback.Continue(path, false);
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IDownloadItemCallback callback)
        {
            if (downloadItem.IsComplete)
            {
                ProjectService.Instance.AddFileAsItem(downloadItem.FullPath, downloadItem.SuggestedFileName);
            }
        }
    }
}
