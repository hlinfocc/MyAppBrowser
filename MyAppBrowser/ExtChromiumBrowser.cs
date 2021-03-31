using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAppBrowser
{
    public class ExtChromiumBrowser : ChromiumWebBrowser
    {
        public ExtChromiumBrowser():base()
        {
            this.LifeSpanHandler = new CefLifeSpanHandler();
            //this.DownloadHandler = new DownloadHandler(this);
        }

        public ExtChromiumBrowser(string url) : base(url)
        {
            this.LifeSpanHandler = new CefLifeSpanHandler();
        }

        public event EventHandler<NewWindowEventArgs> StartNewWindow;

        public void OnNewWindow(NewWindowEventArgs e)
        {
            if (StartNewWindow != null)
            {
                StartNewWindow(this, e);
            }
        }
    }
}
