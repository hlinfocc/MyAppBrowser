using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppBrowser
{
    internal class MyDownloadHandler : IDownloadHandler
    {
        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            //throw new NotImplementedException();
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(@"C:\Users\" +
                            System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                            @"\Downloads\" +
                            downloadItem.SuggestedFileName,
                        showDialog: true);
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            downloadItem.IsCancelled = false;
        }
        public bool OnDownloadUpdated(CefSharp.DownloadItem downloadItem)
        {
            return false;
        }
    }
}
