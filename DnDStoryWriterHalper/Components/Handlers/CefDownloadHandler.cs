using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using DnDStoryWriterHalper.Services;
using HandyControl.Controls;
using HandyControl.Data;

namespace DnDStoryWriterHalper.Components.Handlers
{
    public class CefDownloadHandler : IDownloadHandler
    {
        public bool IsDownloading = false;
        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return !IsDownloading;
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

        public async void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IDownloadItemCallback callback)
        {
            if (downloadItem.IsComplete)
            {
                string? fileName = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var fd = new FileSaveDialogWindow(downloadItem.SuggestedFileName);
                    fd.ShowDialog();
                    fileName = fd.Result;
                });
                if (fileName != null)
                {
                    ProjectService.Instance.AddFileAsItem(downloadItem.FullPath, fileName);
                    Growl.Success(new GrowlInfo()
                    {
                        Message = "Скачено",
                        ShowDateTime = false
                    });
                }
                else
                {
                    Growl.Error(new GrowlInfo()
                    {
                        Message = "Отменено",
                        ShowDateTime = false
                    });
                }

                IsDownloading = false;
            }
            else if(IsDownloading == false)
            {
                IsDownloading = true;
                Growl.Info(new GrowlInfo()
                {
                    Message = "Скачивание начато",
                    WaitTime = 1,
                    ShowDateTime = false
                });
                await Task.Delay(2000);
            }
        }
    }
}
