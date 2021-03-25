using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAppBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var setting = new CefSettings
            {
                Locale = "zh-CN",
                AcceptLanguageList = "zh-CN",
                MultiThreadedMessageLoop = true
            };
            Cef.Initialize(setting);
            this.Text = "呐喊的专栏";
            ChromiumWebBrowser browser = new ChromiumWebBrowser("https://blog.csdn.net/nahancy");
            browser.Dock = DockStyle.Fill;
            //this.Controls.Add(browser);
            browser.TitleChanged += Browser_TitleChanged1;
            this.tabPage1.Controls.Add(browser);
        }
        private void Browser_TitleChanged1(object sender, TitleChangedEventArgs e)
        {
            //throw new NotImplementedException();
            this.Invoke(new MethodInvoker(() =>
            {
                this.tabControl1.SelectedTab.Text = e.Title;
            }));
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
