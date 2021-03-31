using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAppBrowser
{
    public partial class Form1 : Form
    {
        private int tpidx = 2;
        public Form1()
        {
            InitializeComponent();
            CefSettings cefsetting = new CefSettings();
            cefsetting.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache");
            cefsetting.Locale = "zh-CN";
            cefsetting.AcceptLanguageList = "zh-CN";
            cefsetting.MultiThreadedMessageLoop = true;
            cefsetting.CefCommandLineArgs.Add("--disable-web-security", "1");//关闭同源策略,允许跨域
            cefsetting.CefCommandLineArgs.Add("enable-media-stream");
            cefsetting.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            cefsetting.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

            Cef.Initialize(cefsetting);
            this.Text = "呐喊的专栏";
            //ChromiumWebBrowser browser = new ChromiumWebBrowser("https://blog.csdn.net/nahancy");
            ExtChromiumBrowser browser = new ExtChromiumBrowser("https://www.2345.com/?k82494404");
            browser.Dock = DockStyle.Fill;
            browser.StartNewWindow += Browser_StartNewWindow;
            browser.TitleChanged += Browser_TitleChanged1;
            browser.Focus();
            //响应按键
            browser.KeyboardHandler = new CEFKeyBoardHander();
            browser.DownloadHandler = new MyDownloadHandler();
            browser.MenuHandler = new MenuHandler();

            this.tabPage1.Controls.Add(browser);
            this.tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }
        private void Browser_TitleChanged1(object sender, TitleChangedEventArgs e)
        {
            var title = e.Title;
            if (title.Length > 10)
            {
                title = title.Substring(0, 10) + "...";
            }
            //throw new NotImplementedException();
            this.Invoke(new MethodInvoker(() =>
            {
                this.tabControl1.SelectedTab.Text = title;
            }));
        }

        private void Browser_StartNewWindow(object sender, NewWindowEventArgs e)
        {
            TabPage tp = new TabPage();
           
            // tabPage1
            // 
            tp.AutoScroll = true;
            tp.Location = new System.Drawing.Point(4, 26);
            tp.Margin = new System.Windows.Forms.Padding(0);
            tp.Name = "tabPage" + tpidx;
            tp.Size = new System.Drawing.Size(1358, 738);
            tp.TabIndex = tpidx-1;
            tp.Text = "新标签页";
            tp.UseVisualStyleBackColor = true;
            tp.Width = 100;
;
            
            var control = new ExtChromiumBrowser(e.url);
            control.Dock = DockStyle.Fill;
            //control.CreateControl();
            //host.Child = control;
            control.Focus();
            control.StartNewWindow += Browser_StartNewWindow;
            control.TitleChanged += Browser_TitleChanged1;
            control.KeyboardHandler = new CEFKeyBoardHander();
            control.DownloadHandler = new MyDownloadHandler();
            control.MenuHandler = new MenuHandler();

            tp.Controls.Add(control);
            tp.Show();
            this.tabControl1.Controls.Add(tp);
            this.tabControl1.SelectedTab = tp;
            this.tabControl1.SelectedIndex = tpidx - 1;
            this.tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            this.tabControl1.Focus();
            //tbc.Pages.Add(tp);
            //tabFormControl1.SelectedPage = tp;
            //tp.Text = control.Text;
            //e.WindowInfo.SetAsChild(control.Handle, 0, 0, (int)host.ActualWidth, (int)host.ActualHeight);
            tpidx++;
        }

        
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control[] controls = this.tabControl1.SelectedTab.Controls.Find(" tabPage ", false);
             
            foreach (Control control in controls)
            {
                var webBrowser = control as WebBrowser;
                if (webBrowser != null && webBrowser.Document != null)
                {
                    HtmlElementCollection elements = webBrowser.Document.GetElementsByTagName(" body ");
                    if (elements != null && elements.Count > 0)
                    {
                        elements[0].Focus();
                        break;
                    }
                }
            }
        }
    }
}
